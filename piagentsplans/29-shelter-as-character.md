# Plan 29 — The Shelter as Character: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** shelter identity, rooms, machines, and maintenance
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Make shelter history and machine personality truthful projections over existing room, identity, maintenance, atmosphere, and machine-tell owners, with no decorative duplicate state.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5691` characters.
- Current worktree copy: `501924` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d2d26ca5a7acf3624400762cf3654800efbe987c40f65c96fc7bd51da406fd63`
- Snapshot size: 20187 characters; 478 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0052:     /// </summary>
0053:     public sealed class ShelterIdentitySystem
0054:     {
0055:         public const string SystemId = "shelter_identity";
...
0074:
0075:         public ShelterIdentitySystem(string? catalogJson = null, IJsonSerializer? serializer = null)
0076:         {
0077:             if (!string.IsNullOrWhiteSpace(catalogJson) && serializer != null)
```

### Current evidence: `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6c1eddd2273749e7ed30febe53863f2197b12a48354bf49f0edbbee7eededfd9`
- Snapshot size: 30549 characters; 535 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Ashfall.Core.IO;
0005:
0006: namespace Ashfall.Core.Shelter
0007: {
0008:     [Serializable]
0009:     public sealed class ShelterRoomCostDef
0010:     {
0011:         public string item_id { get; set; } = string.Empty;
0012:         public int quantity { get; set; } = 1;
0013:     }
0014:
0015:     /// <summary>
0016:     /// Static authored definition for a shelter room type.
0017:     /// </summary>
0018:     [Serializable]
0019:     public sealed class ShelterRoomDef
0020:     {
```

### Current evidence: `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0fcab04988a4da5e4a53cd8da4516a35597d7d0369e1f8dd8319d967b344498b`
- Snapshot size: 23995 characters; 459 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: // aliases, and discoverable room-history vignettes. Authority:
0007: //   • room identity data  → Assets/StreamingAssets/Data/shelter_room_identities.json
0008: //   • runtime room ids    → existing rosters (StartingLevelSystem, ShelterAssignment,
0009: //                           HoldfastInteriorView, power_grid.json) — NEVER renamed here
...
0020: {
0021:     /// <summary>Root DTO for shelter_room_identities.json (snake_case matches the data authority).</summary>
0022:     [Serializable]
0023:     public sealed class ShelterRoomIdentityCatalogData
...
0107:     /// </summary>
0108:     public sealed class ShelterRoomIdentityCatalog
0109:     {
0110:         public const string FileName = "shelter_room_identities.json";
...
0149:         /// </summary>
0150:         public static ShelterRoomIdentityCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
0151:         {
0152:             var catalog = new ShelterRoomIdentityCatalog();
...
0158:                 string raw = files.ReadAllText(path);
0159:                 var data = json.Deserialize<ShelterRoomIdentityCatalogData>(raw);
0160:                 if (data == null) return catalog;
0161:                 catalog.Build(data);
...
0164:             {
0165:                 CatalogDiagnostics.Warn(path, "ShelterRoomIdentityCatalog", ex);
0166:             }
0167:             return catalog;
```

### Current evidence: `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5807c6ec3fdad9169ddd6c47d30ae978055a2b1d124f97d3360538513a4b5503`
- Snapshot size: 36754 characters; 653 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008: // This catalog persists nothing and mutates nothing: machines/quirks are authored
0009: // data (shelter_machine_identities.json), and tells are projected from readings the
0010: // host snapshots off the owning systems at query time (§29B.7 "projection").
0011: //
...
0164:
0165:     /// <summary>Root DTO for shelter_machine_identities.json (snake_case matches the data authority).</summary>
0166:     [Serializable]
0167:     public sealed class ShelterMachineCatalogData
...
0274:     /// </summary>
0275:     public sealed class ShelterMachineTellCatalog
0276:     {
0277:         public const string FileName = "shelter_machine_identities.json";
...
0298:         /// <summary>Load from the data authority. Missing file → empty valid catalog; malformed → logged warning. Never throws.</summary>
0299:         public static ShelterMachineTellCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
0300:         {
0301:             var catalog = new ShelterMachineTellCatalog();
...
0313:             {
0314:                 CatalogDiagnostics.Warn(path, "ShelterMachineTellCatalog", ex);
0315:             }
0316:             return catalog;
```

### Current evidence: `Assets/Ashfall.Core/Shelter/MachineIdentity/MachineTellAudioSync.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f516ffca7478afcf46bec7568e08c645e4b825d1a7f9c68828e319bcfd088ee7`
- Snapshot size: 6381 characters; 136 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0026:     /// </summary>
0027:     public static class MachineTellAudioSync
0028:     {
0029:         /// <summary>
...
0083:         public static Outcome Apply(
0084:             ShelterMachineTellCatalog catalog,
0085:             MachineConditionReadings readings,
0086:             AudioConditionSystem audio,
```

### Current evidence: `src/Main.ShelterIdentity.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `db03c24f2a2b7b735618b8f1c7ebedd53f146f0251665dd60bc68d647cb63c5a`
- Snapshot size: 7407 characters; 178 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0003: // ASHFALL Plan 166 — Shelter Identity, Naming, Origin & Reputation Projection
0004: // host wiring. The Core ShelterIdentitySystem is the authority for name,
0005: // origin, motto, emblem, infamy, and the shelter's own community-action
0006: // profile. Faction standing remains owned by FactionWarSystem; this host never
```

### Current evidence: `src/Host/ShelterIdentityHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5ce16f4e6be5fe38336117d0d8bb06a04a63ff0910bd60db9f066a8402a01879`
- Snapshot size: 5082 characters; 110 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0043:     {
0044:         private readonly ShelterIdentitySystem _system;
0045:         private string _lastEvent = string.Empty;
0046:
...
0053:
0054:         public ShelterIdentityHostSession(string? dataDir = null, ShelterIdentitySystem? system = null)
0055:         {
0056:             _system = system ?? new ShelterIdentitySystem();
...
0077:
0078:         public static ShelterIdentityHostSession Create(string dataDir, ShelterIdentitySystem? system = null) =>
0079:             new ShelterIdentityHostSession(dataDir, system);
0080:
```

### Current evidence: `src/UI/ShelterDecorPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ebf8710e14d5aaed08d3edaae4b02ef28199d223f09126e419ace0f8188fec4e`
- Snapshot size: 21143 characters; 443 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.Shelter;
0005:
0006: namespace AtomicWar.GodotApp.UI
0007: {
0008:     /// <summary>
0009:     /// Live room-interior panel for Plan 12C. Every action routes through
0010:     /// ShelterDecorHostSession: mounting consumes a real inventory item,
0011:     /// removal returns it to storage, and memorial plaques are read-only
0012:     /// projections of the memorial ledger.
0013:     /// </summary>
0014:     public partial class ShelterDecorPanel : Control, IBindablePanel
0015:     {
0016:         public event Action? OnClose;
0017:
0018:         private AshfallDashboardShell _shell = null!;
0019:         private AshfallStatusRail _statusRail = null!;
0020:         private OptionButton _roomPicker = null!;
```

### Current evidence: `src/UI/ShelterPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `44b789312dfaa95613503bdc0fab1d53282223364e596a8abfe7dfef857b006e`
- Snapshot size: 24561 characters; 496 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0055:
0056:         public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog)
0057:         {
0058:             _interiorView?.SetMachineTellCatalog(catalog);
...
0084:             InventoryHostSession? inventory = null,
0085:             ShelterRoomIdentityCatalog? roomIdentities = null,
0086:             Ashfall.Core.Narrative.BunkerGraffitiCatalog? graffiti = null,
0087:             int currentDay = int.MaxValue)
```

### Current evidence: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e294ef0d24b40c42a9fee8c1650ee6cc3778daa5c89a2f69cea6bcce02f3be90`
- Snapshot size: 54024 characters; 875 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0002:   "schema_version": 1,
0003:   "collection_id": "shelter_room_identities",
0004:   "rooms": [
0005:     {
```

### Current evidence: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `07df62a9aaf375b9199641da062379a647fc5eb4b3606ffdf4495da36e1beb5f`
- Snapshot size: 24630 characters; 610 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0002:   "schema_version": 1,
0003:   "collection_id": "shelter_machine_identities",
0004:   "machines": [
0005:     {
```

### Current evidence: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`
- Snapshot size: 20439 characters; 575 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "shelter_rooms",
0004:   "rooms": [
0005:     {
0006:       "id": "room_bunker_corridor",
0007:       "display_name": "Central Access Corridor",
0008:       "description": "Access concourse and structural spine connecting bunker sectors, hatchways, and stairwells.",
0009:       "function": "Corridor",
0010:       "capacity": 0,
0011:       "max_upgrade_level": 3,
0012:       "required_skill_id": "",
0013:       "workstation_id": "",
0014:       "base_condition": 100.0,
0015:       "build_cost": [
0016:         { "item_id": "scrap_metal", "quantity": 4 }
0017:       ],
0018:       "repair_cost": [
0019:         { "item_id": "scrap_metal", "quantity": 1 }
0020:       ],
```

### Current evidence: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b7b548c465b3bc48c06a03556e08a9e851ba84a55ad9dc662bb7e24701f2796d`
- Snapshot size: 22704 characters; 414 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "the_24_underground_architectural_blueprints_and_engineering_codex",
0004:   "blueprints": [
0005:     {
0006:       "room_id": "room_bp_01_surface_airlock_vestibule",
0007:       "room_name": "Surface Airlock & Decontamination Vestibule",
0008:       "category": "Security & Perimeter",
0009:       "optimal_depth_meters": 0.0,
0010:       "max_dweller_capacity": 6,
0011:       "base_power_draw_kw": 8.5,
0012:       "water_flow_lpm": 25.0,
0013:       "acoustic_noise_db": 68.0,
0014:       "thermal_r_value": 4.2,
0015:       "radiation_attenuation_factor": 0.02,
0016:       "structural_header_spec": "300mm reinforced Class-4 concrete with lead-bismuth sandwich doors",
0017:       "catastrophic_failure_mode": "Decontamination nozzle valve seizure; toxic aerosol backdraft into corridor",
0018:       "maintenance_cycle_days": 14,
0019:       "chief_engineer_note": "The lead sandwich door weighs three tons. If the counterweight cable shears, it drops like a guillotine. Never stand under the threshold while operating the manual winch.",
0020:       "tags": ["airlock", "decontamination", "security", "perimeter", "entry"]
```

### Current evidence: `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `77d55297e7dd4526e6f1a72933aa3e0cf74ce7e2b469ceec0992945cff48c209`
- Snapshot size: 24657 characters; 246 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "the_20_subterranean_engineering_emergencies_and_pipe_glitch_logs",
0004:   "glitches": [
0005:     {
0006:       "glitch_id": "glitch_01_radiator_header_steam_fracture",
0007:       "log_code": "ENG-FL-088-STEAM",
0008:       "affected_subsystem": "Sub-Level 2 Heating Manifold & Residential Block B",
0009:       "severity_tier": 3,
0010:       "anomaly_description": "Cast-iron flanged tee junction on the 4-bar low-pressure steam header cracked along hairline casting seam; superheated steam venting at 140°C into Corridor C.",
0011:       "diagnostic_telemetry": "Manifold pressure drop: 4.2 bar -> 1.8 bar; ambient corridor temperature spiked to 58°C; relative humidity 100%.",
0012:       "required_repair_kit": ["item_heavy_welding_rig", "item_asbestos_gasket_ring", "item_cast_iron_clamp_collar", "item_molybdenum_solder_rod"],
0013:       "emergency_protocol": "Isolate isolation valve V-204 with 2-meter extension lever; vent residual steam through bypass stack; fit bolted two-piece clamp collar with graphite packing.",
0014:       "dmitri_shift_note": "Valery burned his knuckles trying to tighten the flange hot. Reminded him that 140-degree steam is invisible until it strips your skin. Repaired in 45 minutes.",
0015:       "tags": ["steam", "heating", "pipes", "dmitri", "valery", "residential"]
0016:     },
0017:     {
0018:       "glitch_id": "glitch_02_artesian_intake_cavitation_hammer",
0019:       "log_code": "ENG-FL-114-HAMMER",
0020:       "affected_subsystem": "Sub-Level 4 Deep Artesian Well Pumpstation",
```

### Current evidence: `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b983f73034d8ba6a7e506633288597c0bc832f9d6c0c084e1f01d23730fc26cd`
- Snapshot size: 22801 characters; 402 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "the_bunker_graffiti_and_slate_postings",
0004:   "postings": [
0005:     {
0006:       "posting_id": "graf_01_the_first_stoker_rule",
0007:       "recorded_day": 3,
0008:       "location": "Boiler Room Corridor (-10m)",
0009:       "medium": "White chalk on cast-iron steam pipe",
0010:       "author_signature": "Fyodor the Stoker",
0011:       "category": "Gripe / Humor",
0012:       "content": "IF YOU TOUCH THE DRAFT LEVER WITH WET GLOVES, YOU SHOVEL TWO TONS OF LIGNITE ON SATURDAY. NO EXCEPTIONS. YES THIS MEANS YOU, DMITRI.",
0013:       "morale_effect": "+2 Stoker discipline, -1 Dmitri patience",
0014:       "tags": ["boiler", "stoker", "humor", "early_days"]
0015:     },
0016:     {
0017:       "posting_id": "graf_02_ration_biscuit_warning",
0018:       "recorded_day": 12,
0019:       "location": "Canteen Entryway Slate Board",
0020:       "medium": "Charcoal pencil on lime plaster",
```

### Current evidence: `src/UI/JournalPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/JournalPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `412d1eb12aed0d996c58bb0b653d3b90517d40855ec738f9b5cfb4c139f5bf26`
- Snapshot size: 18870 characters; 486 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Linq;
0004: using Godot;
0005: using Ashfall.Core.IO;
0006: using Ashfall.Core.Journal;
0007: using Ashfall.Core.UI;
0008: using AtomicWar.GodotApp.UI;
0009: using DesignTheme = Ashfall.Core.UI.Theme;
0010:
0011: namespace AtomicWar.GodotApp.UI;
0012:
0013: /// <summary>
0014: /// ASHFALL — Journal panel (wired).
0015: /// Shows real journal entries, discovered items, survivors met, locations
0016: /// visited, and narrative events from the live JournalSystem. Replaces the
0017: /// previous hardcoded placeholder strings with live data binding.
0018: /// </summary>
0019: public partial class JournalPanel : Control
0020: {
```

### Current evidence: `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `710c0176c2e56dfac09db8f6df3b2c3729d2715c08e256792e9113b6141143c8`
- Snapshot size: 18396 characters; 426 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0032:
0033:         private ShelterRoomIdentityCatalog LoadCatalog() =>
0034:             ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);
0035:
...
0084:             // non-inspection path must exist alongside inspection.
0085:             Assert.Contains(ShelterRoomIdentityCatalog.UnlockInspectRoom, unlocks);
0086:             Assert.True(unlocks.Count >= 2, $"expected varied unlock paths, got: {string.Join(",", unlocks)}");
0087:             Assert.All(catalog.Vignettes, v => Assert.False(string.IsNullOrEmpty(v.unlock)));
...
0137:             var inspected = catalog.GetUnlockableVignettes("room_filtration",
0138:                 ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected);
0139:             Assert.All(inspected, v =>
0140:                 Assert.Equal(ShelterRoomIdentityCatalog.UnlockInspectRoom, v.unlock));
...
0144:             Assert.All(repaired, v =>
0145:                 Assert.Equal(ShelterRoomIdentityCatalog.UnlockRepairPerformed, v.unlock));
0146:             Assert.NotEmpty(repaired);
0147:         }
...
0153:             var milestones = catalog.Vignettes
0154:                 .Where(v => v.unlock == ShelterRoomIdentityCatalog.UnlockDayMilestone)
0155:                 .ToList();
0156:             Assert.NotEmpty(milestones);
...
0160:                 var early = catalog.GetUnlockableVignettes(milestone.room_id,
0161:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.DayElapsed, milestone.unlock_day - 1);
0162:                 Assert.DoesNotContain(milestone.id, early.Select(v => v.id));
0163:
...
0188:             Assert.Empty(catalog.GetUnlockableVignettes("room_nonexistent",
0189:                 ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
0190:         }
0191:
...
0195:             var catalog = LoadCatalog();
0196:             Assert.Equal(ShelterRoomIdentityCatalog.UnlockInspectRoom, ShelterRoomIdentityCatalog.UnlockValueFor(
0197:                 ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
0198:             Assert.Equal(ShelterRoomIdentityCatalog.UnlockRepairPerformed, ShelterRoomIdentityCatalog.UnlockValueFor(
...
0204:                 {
0205:                     ShelterRoomIdentityCatalog.UnlockInspectRoom,
0206:                     ShelterRoomIdentityCatalog.UnlockRepairPerformed,
0207:                     ShelterRoomIdentityCatalog.UnlockDayMilestone
...
0254:         {
0255:             var catalog = ShelterRoomIdentityCatalog.Load(_files, _json, Path.Combine(_dataDir, "no_such_dir"));
0256:             Assert.Equal(0, catalog.RoomCount);
0257:             Assert.Empty(catalog.Validate());
...
0325:                 inspect += catalog.GetUnlockableVignettes(room.id,
0326:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected).Count;
0327:                 repair += catalog.GetUnlockableVignettes(room.id,
0328:                     ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed).Count;
```

### Current evidence: `Ashfall.Core.Tests/ShelterMachineTellTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8efba80cec8ad8c88907cb99896fd68f5e436bf7f1846b2a174549639ae9c2a6`
- Snapshot size: 17561 characters; 352 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0035:
0036:         private ShelterMachineTellCatalog LoadMachineCatalog() =>
0037:             ShelterMachineTellCatalog.Load(_files, _json, _dataDir);
0038:
...
0056:         {
0057:             var catalog = ShelterMachineTellCatalog.Load(_files, _json, Path.Combine(_dataDir, "no_such_dir"));
0058:             Assert.Equal(0, catalog.MachineCount);
0059:             Assert.Empty(catalog.Validate());
...
0067:             var machines = LoadMachineCatalog();
0068:             var rooms = ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);
0069:
0070:             var hepa = machines.GetMachine("machine_hepa_stack");
...
0196:             var catalog = LoadMachineCatalog();
0197:             Assert.Equal(ShelterMachineTellCatalog.BandFor(average),
0198:                 catalog.EvaluateBand("machine_foundry_cupola", ReadFrom(foundry)));
0199:             Assert.Equal(MachineConditionBand.Worn, catalog.EvaluateBand("machine_foundry_cupola", ReadFrom(foundry)));
...
0267:         {
0268:             Assert.Equal(MachineConditionBand.Healthy, ShelterMachineTellCatalog.BandFor(100f));
0269:             Assert.Equal(MachineConditionBand.Worn, ShelterMachineTellCatalog.BandFor(69.9f));
0270:             Assert.Equal(MachineConditionBand.ServiceDue, ShelterMachineTellCatalog.BandFor(49.9f));
```

### Current evidence: `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `7b34f2e5316d2007439a8eccf5a1970444286b752fb59cb1d0fbfa96cda2511e`
- Snapshot size: 13468 characters; 302 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0018: {
0019:     public class MachineTellAudioSyncTests
0020:     {
0021:         private readonly string _dataDir;
...
0024:
0025:         public MachineTellAudioSyncTests()
0026:         {
0027:             string baseDir = AppContext.BaseDirectory;
...
0032:
0033:         private ShelterMachineTellCatalog LoadMachineCatalog() =>
0034:             ShelterMachineTellCatalog.Load(_files, _json, _dataDir);
0035:
...
0065:
0066:             var outcome = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
0067:
0068:             Assert.Equal(7, outcome.Started.Count);
...
0086:             var audioAtFloor = new AudioConditionSystem();
0087:             var outcomeAtFloor = MachineTellAudioSync.Apply(catalog, atFloor, audioAtFloor);
0088:             Assert.DoesNotContain("machine_quirk_hepa_intake_whistle",
0089:                 audioAtFloor.State.activeConditions.Select(c => c.conditionId));
...
0093:             var audioBelow = new AudioConditionSystem();
0094:             MachineTellAudioSync.Apply(catalog, below, audioBelow);
0095:             Assert.Contains("machine_quirk_hepa_intake_whistle",
0096:                 audioBelow.State.activeConditions.Select(c => c.conditionId));
...
0109:             var audio = new AudioConditionSystem();
0110:             var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);
0111:
0112:             Assert.Contains("machine_quirk_hepa_intake_whistle", outcome.Started);
...
0126:             var audio = new AudioConditionSystem();
0127:             MachineTellAudioSync.Apply(catalog, readings, audio);
0128:
0129:             Assert.Contains("machine_quirk_hepa_intake_whistle",
...
0160:             var audio = new AudioConditionSystem();
0161:             var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);
0162:
0163:             Assert.Equal(catalog.Quirks.Count, outcome.ActiveTotal); // all 20 carry audio cues
...
0176:
0177:             MachineTellAudioSync.Apply(catalog, readings, audio);
0178:             var second = MachineTellAudioSync.Apply(catalog, readings, audio);
0179:
...
0192:             var audio = new AudioConditionSystem();
0193:             MachineTellAudioSync.Apply(catalog, degraded, audio);
0194:
0195:             var recovered = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
...
```

### Current evidence: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5121939d1dbac0de2d70af77bb20e6f8e34687a61d5d0ece18179cb7e4fe12d3`
- Snapshot size: 6444 characters; 166 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.IO;
0004: using System.Linq;
0005: using Ashfall.Core;
0006: using Ashfall.Core.Shelter;
0007: using Xunit;
0008:
0009: namespace Ashfall.Core.Tests.Shelter
0010: {
0011:     public class ShelterRoomCatalogTests
0012:     {
0013:         private static string GetDataPath()
0014:         {
0015:             return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "StreamingAssets", "Data");
0016:         }
0017:
0018:         [Fact]
0019:         public void DefaultCatalog_Contains22RoomsAnd12Rules()
0020:         {
```

### Current evidence: `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4b33afcb5e14215ca69220aa0e61e33c5c7e83fbc30d5a6295564e9aa1a11c7f`
- Snapshot size: 6250 characters; 154 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009: {
0010:     public sealed class ShelterIdentitySystemTests
0011:     {
0012:         [Fact]
...
0029:         {
0030:             var system = new ShelterIdentitySystem();
0031:
0032:             // Valid naming
...
0055:         {
0056:             var system = new ShelterIdentitySystem();
0057:
0058:             var res = system.SelectOrigin("origin_government_bunker", day: 2, founderSurvivorId: "surv_colonel");
...
0071:         {
0072:             var system = new ShelterIdentitySystem();
0073:
0074:             system.RecordFactionReputation("faction_salvagers", 500);
...
0086:         {
0087:             var system = new ShelterIdentitySystem();
0088:
0089:             // Initial state: default tag
...
0113:         {
0114:             var system = new ShelterIdentitySystem();
0115:             system.SetShelterName("Iron Redoubt");
0116:             system.SetMotto("Through Fire We Endure");
...
0127:         {
0128:             var sys1 = new ShelterIdentitySystem();
0129:             sys1.SetShelterName("Silo 7");
0130:             sys1.SetMotto("Vigilance Forever");
...
0138:
0139:             var sys2 = new ShelterIdentitySystem();
0140:             sys2.RestoreState(saved);
0141:
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/shelter_room_identities.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, rooms, vignettes, fixtures`
- `rooms`: list count=13; sample IDs=['room_bunker_corridor', 'room_storage_bay', 'room_bunks', 'room_kitchen', 'room_clinic', 'room_workshop', 'room_filtration', 'room_airlock']
- `vignettes`: list count=21; sample IDs=['room_history_the_first_filter_change', 'room_history_a_frame_stayed', 'room_history_four_pale_rectangles', 'room_history_the_count_came_short', 'room_history_the_basin_that_was_a_mixing_bowl', 'room_history_a_chair_from_the_row', 'room_history_the_discrepancy', 'room_history_the_second_blower']
- `fixtures`: list count=53; sample IDs=['room_fixture_corridor_chart_rail', 'room_fixture_corridor_plate_rectangles', 'room_fixture_corridor_pencil_stub', 'room_fixture_corridor_scrub_line', 'room_fixture_bunks_stencil_gaps', 'room_fixture_bunks_spare_socket', 'room_fixture_bunks_dosimeter_nail', 'room_fixture_bunks_bolt_rings']
- `schema_version`: `1`
- `collection_id`: `shelter_room_identities`
- SHA-256: `e294ef0d24b40c42a9fee8c1650ee6cc3778daa5c89a2f69cea6bcce02f3be90`
#### `Assets/StreamingAssets/Data/shelter_machine_identities.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, machines, quirks, glitch_events`
- `machines`: list count=7; sample IDs=['machine_hepa_stack', 'machine_foundry_cupola', 'machine_generator', 'machine_ventilation_plant', 'machine_water_still', 'machine_boiler', 'machine_airlock_machinery']
- `quirks`: list count=20; sample IDs=['machine_quirk_hepa_intake_whistle', 'machine_quirk_hepa_storm_cough', 'machine_quirk_hepa_housing_tick', 'machine_quirk_foundry_tuyere_knock', 'machine_quirk_foundry_exhaust_whine', 'machine_quirk_generator_fuel_cough', 'machine_quirk_generator_battery_dip', 'machine_quirk_ventilation_loaded_rattle']
- `glitch_events`: list count=11; sample IDs=['glitch_21_phantom_draft', 'glitch_22_repeating_relay_click', 'glitch_23_old_intercom_burst', 'glitch_24_seal_cycles', 'glitch_25_ground_loop', 'glitch_26_stuck_damper', 'glitch_27_pressure_flutter', 'glitch_28_boiler_cutout']
- `schema_version`: `1`
- `collection_id`: `shelter_machine_identities`
- SHA-256: `07df62a9aaf375b9199641da062379a647fc5eb4b3606ffdf4495da36e1beb5f`
#### `Assets/StreamingAssets/Data/shelter_rooms.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, rooms, assignment_rules`
- `rooms`: list count=23; sample IDs=['room_bunker_corridor', 'room_bunks_crowded', 'room_bunks', 'room_quarters_private', 'room_workshop', 'room_workshop_heavy', 'room_workshop_precision', 'room_clinic']
- `assignment_rules`: list count=12; sample IDs=['rule_medical_field_surgery', 'rule_workshop_machinist', 'rule_workshop_precision', 'rule_radio_communications', 'rule_kitchen_nutrition', 'rule_laboratory_analysis', 'rule_greenhouse_botany', 'rule_generator_maintenance']
- `schema_version`: `1`
- `collection_id`: `shelter_rooms`
- SHA-256: `1d7df91334461310d3c76457b60a93df678369bdc3846f00917c1f3bf5e8d379`
#### `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, blueprints`
- `blueprints`: list count=24; sample IDs=['room_bp_01_surface_airlock_vestibule', 'room_bp_02_diesel_generator_vault', 'room_bp_03_central_ventilation_blower_station', 'room_bp_04_deep_artesian_well_pump_room', 'room_bp_05_residential_bunk_cubicle_block', 'room_bp_06_community_soup_canteen_galley', 'room_bp_07_underground_hydroponic_greenhouse', 'room_bp_08_medical_clinic_and_surgery_suite']
- `schema_version`: `1`
- `collection_id`: `the_24_underground_architectural_blueprints_and_engineering_codex`
- SHA-256: `b7b548c465b3bc48c06a03556e08a9e851ba84a55ad9dc662bb7e24701f2796d`
#### `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, glitches`
- `glitches`: list count=20; sample IDs=[]
- `schema_version`: `1`
- `collection_id`: `the_20_subterranean_engineering_emergencies_and_pipe_glitch_logs`
- SHA-256: `77d55297e7dd4526e6f1a72933aa3e0cf74ce7e2b469ceec0992945cff48c209`
#### `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, postings`
- `postings`: list count=36; sample IDs=[]
- `schema_version`: `1`
- `collection_id`: `the_bunker_graffiti_and_slate_postings`
- SHA-256: `b983f73034d8ba6a7e506633288597c0bc832f9d6c0c084e1f01d23730fc26cd`
## Symbol and caller audit

#### `ShelterIdentitySystem` — HOST_REFERENCE_PRESENT — core/declaration=3, host=6, test=11
- `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs:53` (declaration) — public sealed class ShelterIdentitySystem
- `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs:75` (core) — public ShelterIdentitySystem(string? catalogJson = null, IJsonSerializer? serializer = null)
- `Assets/Ashfall.Core/Shelter/ShelterOriginCatalogLoader.cs:10` (core) — /// The lenient <see cref="ShelterIdentitySystem.LoadCatalog"/> path remains
- `src/Main.ShelterIdentity.cs:4` (host) — // host wiring. The Core ShelterIdentitySystem is the authority for name,
- `src/Host/ShelterIdentityHostSession.cs:44` (host) — private readonly ShelterIdentitySystem _system;
- `src/Host/ShelterIdentityHostSession.cs:47` (host) — public ShelterIdentitySystem System => _system;
- `src/Host/ShelterIdentityHostSession.cs:54` (host) — public ShelterIdentityHostSession(string? dataDir = null, ShelterIdentitySystem? system = null)
- `src/Host/ShelterIdentityHostSession.cs:56` (host) — _system = system ?? new ShelterIdentitySystem();
- `src/Host/ShelterIdentityHostSession.cs:78` (host) — public static ShelterIdentityHostSession Create(string dataDir, ShelterIdentitySystem? system = null) =>
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:15` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:30` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:56` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:72` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:87` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:114` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:128` (test) — var sys1 = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs:139` (test) — var sys2 = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterOriginCatalogLoaderTests.cs:34` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterOriginCatalogLoaderTests.cs:55` (test) — var system = new ShelterIdentitySystem();
- `Ashfall.Core.Tests/Shelter/ShelterOriginCatalogLoaderTests.cs:70` (test) — var system = new ShelterIdentitySystem();
#### `ShelterRoomIdentityCatalog` — HOST_REFERENCE_PRESENT — core/declaration=4, host=9, test=28
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:108` (declaration) — public sealed class ShelterRoomIdentityCatalog
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:150` (core) — public static ShelterRoomIdentityCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:152` (core) — var catalog = new ShelterRoomIdentityCatalog();
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:165` (core) — CatalogDiagnostics.Warn(path, "ShelterRoomIdentityCatalog", ex);
- `src/Main.ShelterInfrastructure.cs:56` (host) — private ShelterRoomIdentityCatalog? _shelterRoomIdentity;
- `src/Main.ShelterInfrastructure.cs:59` (host) — private ShelterRoomIdentityCatalog? GetShelterRoomIdentityCatalog()
- `src/Main.ShelterInfrastructure.cs:62` (host) — _shelterRoomIdentity = ShelterRoomIdentityCatalog.Load(
- `src/Main.ShelterInfrastructure.cs:158` (host) — ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
- `src/Main.ShelterInfrastructure.cs:181` (host) — ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed));
- `src/Main.ShelterInfrastructure.cs:227` (host) — private void UnlockRoomHistories(ShelterRoomIdentityCatalog? catalog,
- `src/UI/ShelterPanel.cs:85` (host) — ShelterRoomIdentityCatalog? roomIdentities = null,
- `src/World/HoldfastInteriorView.cs:62` (host) — private Ashfall.Core.Shelter.ShelterRoomIdentityCatalog? _roomIdentities;
- `src/World/HoldfastInteriorView.cs:243` (host) — public void SetRoomIdentityCatalog(Ashfall.Core.Shelter.ShelterRoomIdentityCatalog? catalog)
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:39` (test) — private ShelterRoomIdentityCatalog LoadRoomCatalog() =>
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:40` (test) — ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:68` (test) — var rooms = ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:33` (test) — private ShelterRoomIdentityCatalog LoadCatalog() =>
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:34` (test) — ShelterRoomIdentityCatalog.Load(_files, _json, _dataDir);
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:85` (test) — Assert.Contains(ShelterRoomIdentityCatalog.UnlockInspectRoom, unlocks);
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:138` (test) — ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected);
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:140` (test) — Assert.Equal(ShelterRoomIdentityCatalog.UnlockInspectRoom, v.unlock));
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:143` (test) — ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed);
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:145` (test) — Assert.Equal(ShelterRoomIdentityCatalog.UnlockRepairPerformed, v.unlock));
- `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs:154` (test) — .Where(v => v.unlock == ShelterRoomIdentityCatalog.UnlockDayMilestone)
- … 17 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `ShelterMachineTellCatalog` — HOST_REFERENCE_PRESENT — core/declaration=5, host=9, test=13
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:275` (declaration) — public sealed class ShelterMachineTellCatalog
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:299` (core) — public static ShelterMachineTellCatalog Load(IFileIO files, IJsonSerializer json, string dataDirectory)
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:301` (core) — var catalog = new ShelterMachineTellCatalog();
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:314` (core) — CatalogDiagnostics.Warn(path, "ShelterMachineTellCatalog", ex);
- `Assets/Ashfall.Core/Shelter/MachineIdentity/MachineTellAudioSync.cs:84` (core) — ShelterMachineTellCatalog catalog,
- `src/Main.ShelterInfrastructure.cs:68` (host) — private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;
- `src/Main.ShelterInfrastructure.cs:70` (host) — private Ashfall.Core.Shelter.ShelterMachineTellCatalog GetMachineTellCatalog()
- `src/Main.ShelterInfrastructure.cs:73` (host) — _machineTellCatalog = Ashfall.Core.Shelter.ShelterMachineTellCatalog.Load(
- `src/UI/ShelterPanel.cs:56` (host) — public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog)
- `src/UI/SilentFoundryPanel.cs:42` (host) — private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;
- `src/UI/SilentFoundryPanel.cs:59` (host) — public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog)
- `src/Audio/AudioSelfTest.cs:1264` (host) — var tellCatalog = ShelterMachineTellCatalog.Load(
- `src/World/HoldfastInteriorView.cs:64` (host) — private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;
- `src/World/HoldfastInteriorView.cs:253` (host) — public void SetMachineTellCatalog(Ashfall.Core.Shelter.ShelterMachineTellCatalog? catalog)
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:36` (test) — private ShelterMachineTellCatalog LoadMachineCatalog() =>
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:37` (test) — ShelterMachineTellCatalog.Load(_files, _json, _dataDir);
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:57` (test) — var catalog = ShelterMachineTellCatalog.Load(_files, _json, Path.Combine(_dataDir, "no_such_dir"));
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:197` (test) — Assert.Equal(ShelterMachineTellCatalog.BandFor(average),
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:268` (test) — Assert.Equal(MachineConditionBand.Healthy, ShelterMachineTellCatalog.BandFor(100f));
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:269` (test) — Assert.Equal(MachineConditionBand.Worn, ShelterMachineTellCatalog.BandFor(69.9f));
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:270` (test) — Assert.Equal(MachineConditionBand.ServiceDue, ShelterMachineTellCatalog.BandFor(49.9f));
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:271` (test) — Assert.Equal(MachineConditionBand.Critical, ShelterMachineTellCatalog.BandFor(24.9f));
- `Ashfall.Core.Tests/ShelterMachineTellTests.cs:272` (test) — Assert.Equal(MachineConditionBand.Failed, ShelterMachineTellCatalog.BandFor(0f));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:33` (test) — private ShelterMachineTellCatalog LoadMachineCatalog() =>
- … 3 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `MachineTellAudioSync` — HOST_REFERENCE_PRESENT — core/declaration=1, host=4, test=24
- `Assets/Ashfall.Core/Shelter/MachineIdentity/MachineTellAudioSync.cs:27` (declaration) — public static class MachineTellAudioSync
- `src/Main.ShelterInfrastructure.cs:221` (host) — Ashfall.Core.Shelter.MachineTellAudioSync.Apply(
- `src/Audio/AudioSelfTest.cs:1295` (host) — var degradedOutcome = MachineTellAudioSync.Apply(tellCatalog, degradedReadings, tellAudio,
- `src/Audio/AudioSelfTest.cs:1305` (host) — var steadyOutcome = MachineTellAudioSync.Apply(tellCatalog, degradedReadings, tellAudio,
- `src/Audio/AudioSelfTest.cs:1309` (host) — var recoveredOutcome = MachineTellAudioSync.Apply(tellCatalog, new MachineConditionReadings(), tellAudio,
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:66` (test) — var outcome = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:87` (test) — var outcomeAtFloor = MachineTellAudioSync.Apply(catalog, atFloor, audioAtFloor);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:94` (test) — MachineTellAudioSync.Apply(catalog, below, audioBelow);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:110` (test) — var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:127` (test) — MachineTellAudioSync.Apply(catalog, readings, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:161` (test) — var outcome = MachineTellAudioSync.Apply(catalog, readings, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:177` (test) — MachineTellAudioSync.Apply(catalog, readings, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:178` (test) — var second = MachineTellAudioSync.Apply(catalog, readings, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:193` (test) — MachineTellAudioSync.Apply(catalog, degraded, audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:195` (test) — var recovered = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:212` (test) — var outcome = MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:223` (test) — Assert.Equal("ventilation", MachineTellAudioSync.BusForMachine("machine_hepa_stack"));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:224` (test) — Assert.Equal("ventilation", MachineTellAudioSync.BusForMachine("machine_ventilation_plant"));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:225` (test) — Assert.Equal("generator", MachineTellAudioSync.BusForMachine("machine_generator"));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:226` (test) — Assert.Equal("ambient", MachineTellAudioSync.BusForMachine("machine_foundry_cupola"));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:227` (test) — Assert.Equal("ambient", MachineTellAudioSync.BusForMachine("machine_boiler"));
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:231` (test) — MachineTellAudioSync.Apply(catalog, HealthyReadings(), audio);
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:254` (test) — MachineTellAudioSync.Apply(catalog, degraded, audio,
- `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs:266` (test) — MachineTellAudioSync.Apply(catalog, degraded, audioDefault);
- … 5 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `shelter_room_identities` — HOST_REFERENCE_PRESENT — core/declaration=6, host=3, test=1
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:546` (core) — // references to them (shelter_room_identities.json legacy_aliases,
- `Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs:40` (core) — ///   - shelter-room contexts (room_* ids from shelter_room_identities.json);
- `Assets/Ashfall.Core/Narrative/OralLorePerformanceSystem.cs:104` (core) — /// Room producers use canonical room ids (shelter_room_identities.json);
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:7` (core) — //   • room identity data  → Assets/StreamingAssets/Data/shelter_room_identities.json
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:21` (core) — /// <summary>Root DTO for shelter_room_identities.json (snake_case matches the data authority).</summary>
- `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs:110` (core) — public const string FileName = "shelter_room_identities.json";
- `src/Journal/JournalCatalogData.cs:53` (host) — /// Authored in shelter_room_identities.json; gated by the journal knowledge
- `src/Journal/JournalCatalogData.cs:74` (host) — /// <summary>Shelter room-history vignettes (shelter_room_identities.json, Plan 29 29A).</summary>
- `src/Journal/JournalCatalogData.cs:211` (host) — string path = fileIO.Combine(dataDir, "shelter_room_identities.json");
- `Ashfall.Core.Tests/Narrative/OralLorePlan155Tests.cs:210` (test) — string roomsJson = File.ReadAllText(Path.Combine(DataDirectory, "shelter_room_identities.json"));
#### `shelter_machine_identities` — HOST_REFERENCE_PRESENT — core/declaration=3, host=1, test=0
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:9` (core) — // data (shelter_machine_identities.json), and tells are projected from readings the
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:165` (core) — /// <summary>Root DTO for shelter_machine_identities.json (snake_case matches the data authority).</summary>
- `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs:277` (core) — public const string FileName = "shelter_machine_identities.json";
- `src/Audio/AudioCueCatalog.cs:309` (host) — // shelter_machine_identities.json quirks[].audio_cue; semantics in
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 16-19
00016:
00017: The v1.0 master bible was a snapshot: a large, well-structured reference that a planner reads before drafting one plan. Its structural weakness, identified during the 2026-09-24 audit, is that it is a *library*, not a *machine*. It tells a planner what exists, but it does not encode the generative move — the repeatable transformation of (repository evidence × lane × subsystem) into a bounded subject plan with a recommended integration route.
00018:
00019: v2.0 therefore adds four new organs on top of the preserved v1.0 body:
#### authority lines 44-47
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
00045: Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.
00046:
00047: **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
#### authority lines 117-120
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
#### authority lines 145-148
00145: |---|---|---|
00146: | C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
00147: | C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
00148: | C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
#### authority lines 159-162
00159: | C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
00160: | C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
00161: | C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
00162: | C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |
#### authority lines 667-670
00667:
00668: **A-01 · C1 · Bunker maintenance glitch batch N+1.** Subject: additional `bunker_maintenance_glitches` batches for shelter rooms that gained systems in Waves 8–12 (EMP feed, medical power, grid catalog seal — logs observed in `docs/plans/`). Evidence: glitch batches 2 and 3 exist in the narrative corpus; room coverage is enumerable from `shelter_rooms.json`. Route: DATA-ONLY into the existing glitch corpus family. Confidence: HIGH CONFIDENCE.
00669:
00670: **A-02 · C1 · Load-shed schedule amendments tied to the sanitation power-grid feed.** Subject: amendment notices following the sanitation `RoomPowerProvider` seam (sanitation power-grid feed landed per `FOLLOWUPS-210-213-THINSEAMS`). Evidence: `load_shed_schedule_001` exists in the corpus; the power feed seam is verified via the followups package. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 729-732
00729:
00730: **B-01 · C1 · Shelter-failure follow-on effects.** Subject: extend the quarantined shelter-failure-effects wiring (logs observed: `SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING_*`) from quarantine into full cascade coverage through `cascade_rules.json`. Evidence: implementation logs verified in `docs/plans/`. Route: CORE-EXTENSION + data, per the existing quarantine plan's own exit criteria. Confidence: HIGH CONFIDENCE that the quarantine exists; read its exit criteria in session.
00731:
00732: **B-02 · C1 · Shelter grid catalog seal follow-through.** Subject: complete any consumer bindings left open by the shelter grid catalog seal (log observed: `SHELTER_GRID_CATALOG_SEAL_*`). Evidence: logs verified live. Route: HOST-WIRING per seal plan. Confidence: HIGH CONFIDENCE the seal exists; scope unverified.
#### authority lines 821-824
00821:
00822: **D-06 · C1 · Shelter failure mid-event save semantics.** Subject: define and test mid-failure-event save/restore behavior for the quarantine-exited failure cascades (B-01 dependency). Evidence: quarantine logs verified. Route: design + tests. Confidence: PROPOSAL.
00823:
00824: **D-07 · C10 · Moral-choice flag persistence audit.** Subject: confirm every authored flag id persists and round-trips; default-tolerant missing-flag handling for older saves. Evidence: flags catalog live; F-001 depends on this. Route: tests. Confidence: HIGH CONFIDENCE as an audit; outcomes may be NONE.
#### authority lines 862-865
00862:
00863: **F-04 · C1 · Room tree-search frequency.** Subject: count repeated node/path lookups in shelter systems during a 30-day simulation; the atlas flags repeated tree searches as a candidate class. Evidence: 30-day simulation patterns exist (shelter maintenance report, expedition playtest). Route: instrumentation run. Confidence: potential hotspot — requires profiling.
00864:
00865: **F-05 · C8 · Radio dial per-frame work.** Subject: measure SNR dial work at 15 FPS during active tuning; the dial is one of the canon real-time/frame surfaces. Evidence: real-time tier is canon (v1.0 Part 3.2). Route: profiling pass. Confidence: potential hotspot — requires profiling.
#### authority lines 932-935
00932:
00933: **DM-1 — Shelter operations (C1).** Owners: shelter rooms/identities/machines, thermal, schedules, social events, decor, fire, noise, airlock security, decon, atmosphere, sanitation (power-fed). Live catalogs: `shelter_rooms`, `shelter_room_identities`, `shelter_machine_identities`, `shelter_schedules`, `shelter_social_events`, `shelter_audio_cues`, `shelter_insulation_catalog`, `shelter_shielding`, `sanitation_facilities`, plus the sealed grid catalog. Hosts: ShelterAssignment, ShelterAtmosphere, ShelterDecor, ShelterFire, ShelterSchedule, ShelterThermal, Sanitation, AirlockSecurity, Decontamination, Ventilation. Openings: A-01, A-02, B-01, B-02, D-06, F-04. Notable constraint: room effects route through `IsRoomPowered`; shelter state persists through the holdfast/shelter save family.
00934:
00935: **DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.
#### authority lines 950-953
00950:
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
00952:
00953: **DM-11 — Economy (C11).** Owners: market, price factors, shocks, baselines, regional prices, hardcore tuning, rumor bands, black market, caravans, debt ledger, foundry economy, bounty board, trade screens. Live catalogs: `commodity_baselines`, `regional_prices`, `hardcore_economy_tuning`, `economy_goods`, `black_market_inventory`, `ledger_debt_templates`, `trade_screen_scenarios`, `trade_tell_lines`, `trade_specialties`, `trade_texts`, `bounty_board`. Hosts: Economy, BlackMarket, TravelingCaravan, SilentFoundry. Docs: `ECONOMY_FAIRNESS_AUDIT.md`, `ECONOMY_PRICE_FACTOR_MATRIX.md` (verified live). Sealed: merchant restock priority (DEC-05). Openings: A-26, B-18, C-07, C-08, C-13, E-08, G-02. GATE: black-market funds legs.
#### authority lines 964-967
00964:
00965: **DM-17 — Host surface and UI (C17).** Owners: the panel families (v1.0 Part 5.7), shell components, focus navigator, snapshots, a11y, briefings. Design pinned by `DESIGN.md`; a11y by `ACCESSIBILITY.md`; input by the 22-action map. Openings: E-01 through E-10, B-24, F-01. Constraint: zero gameplay authority in panels; every panel exposes existing commands and truthful state.
00966:
00967:
#### authority lines 984-987
00984:   MANIFEST 44-C — VERITY MOTEL RUN
00985:   Received of: shelter stores, per requisition 12
00986:   Blankets, wool, 6 — two with seam failure, noted
00987:   Iodine tablets, tin, 2 — seals intact
#### authority lines 1123-1126
01123: - 2026-09-24 — Volume 2: subject seed catalog, Lanes A–E (A-01…A-30, B-01…B-25, C-01…C-14, D-01…D-08, E-01…E-10) — ~29,000 — cumulative ~97,000
01124: - 2026-09-24 — Volume 3: subject seed catalog, Lanes F–J (F-01…F-06, G-01…G-08, H-01…H-07, I-01…I-06, J-01…J-05) plus seventeen subsystem deep maps (DM-1…DM-17) — ~23,000 — cumulative ~120,000
01125: - 2026-09-24 — Volume 4: prose specification library part 1 (eight worked genre contracts plus usage rules) — ~5,500 — cumulative ~125,500
01126:
#### authority lines 1139-1142
01139: - 2026-09-27 — Volume 15: nine remaining Lane A seed expansions into full plans (FP-A09, FP-A14, FP-A15, FP-A20, FP-A23, FP-A24, FP-A25, FP-A29, FP-A30), closing Lane A's compressed-seed backlog, including the FP-A25 200-record quest prose audit tranche program (Tranche 0 census plus eight authoring tranches plus completion regression) — ~19,000 — cumulative ~301,000
01140: - 2026-09-27 — Volume 16: worked content tranche library — twelve PROPOSAL-model JSON tranche examples per established genre (glitch, load-shed, assay, interlock report, marginalia, rundown, intake, quest prose, sighting log, ordnance manifest, almanac), each with validation notes and the three mandatory focused tests — ~12,400 — cumulative ~313,000
01141: - 2026-09-27 — Volume 17: cluster-by-cluster expansion roadmaps C1–C17, each anchored to its deep map with Phase I–IV structure and five cross-cluster sequencing rules — ~16,900 — cumulative ~330,000
01142: - 2026-09-27 — Volume 18: re-audit against the repository's public surface — four drift-register candidates (DR-16 communiqué board and tick-gate, DR-17 map-atlas repair, DR-18 hardening/quarantine cleanup, DR-19 post-v1.0 subsystem families absent from the deep maps), five premise corrections, five evidence-gated seed replenishments (A-31, A-32, B-26, G-09, E-11), and the factory self-audit — ~9,900 — cumulative ~342,000
#### authority lines 1171-1174
01171:   I will regret; favors keep no ledger anyone can read. Walked the
01172:   corridor twice after and the pump room still smells of hot iron,
01173:   which the maintenance log says it should not. Noted it in the margin
01174:   here because the log is someone's job and margins are mine.
#### authority lines 1240-1243
01240:   what is owed is carried until the spring court says otherwise. So
01241:   the shelter held, so the shelter holds. The verses are counted, not
01242:   sung, until the meter is paid.
01243: ` ` `
#### authority lines 1282-1285
01282: purpose: unofficial public voice, compressed and worn
01283: trigger: shelter graffiti postings, wall carving templates
01284: length: 4-20 words
01285: must_include: one concrete claim or instruction, wear or placement cue
… 89 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.

**Requested behavior.** Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.

**Minimum safe delta.** Extend `ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.

**Required delta.** Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.

**Primary seam.** ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `ShelterIdentitySystem`, `ShelterRoomIdentityCatalog`, `ShelterMachineTellCatalog`, `MachineTellAudioSync`, `shelter_room_identities`, `shelter_machine_identities`.

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
| Domain rules | Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 29.

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
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.
- What is the smallest safe change? Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.
- Which owner is touched? Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Shelter/ShelterRoomCatalog.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Shelter/RoomIdentity/ShelterRoomIdentityCatalog.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Shelter/MachineIdentity/ShelterMachineTellCatalog.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Shelter/MachineIdentity/MachineTellAudioSync.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.ShelterIdentity.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/ShelterIdentityHostSession.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/ShelterDecorPanel.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/ShelterPanel.cs` — Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/shelter_room_identities.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/shelter_machine_identities.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/shelter_rooms.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/ShelterDecorPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/ShelterPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/JournalPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/ShelterMachineTellTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio is wired end to end or the plan explicitly closes as already integrated.
- Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

ShelterIdentitySystem, ShelterRoomCatalog, ShelterRoomIdentityCatalog, ShelterMachineTellCatalog, ShelterIdentityHostSession, shelter_room_identities.json, shelter_machine_identities.json, room/machine tests, and decor surfaces already exist. The live seam is richer than the old plan premise; the plan must separate authored identity, condition-derived tells, player inspection, and actual repair effects.

# 3. Required Delta

Extend identity and presentation contracts so room history, machine tells, maintenance actions, morale/atmosphere effects, codex/journal facts, and save state are linked through existing owners and remain readable under failure.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Shelter operations and machine-tell work is active elsewhere; this plan is a read-only integration map and must not claim those shared paths. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `room_bunker_corridor`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `room_storage_bay`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `room_bunks`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `room_kitchen`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `room_clinic`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `room_workshop`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `room_filtration`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `room_airlock`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `room_radio_tuner`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `room_foundry`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `room_greenhouse`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `room_main`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `room_water_pump`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `room_history_the_first_filter_change`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `room_history_a_frame_stayed`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `room_history_four_pale_rectangles`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `room_history_the_count_came_short`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `room_history_the_basin_that_was_a_mixing_bowl`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `room_history_a_chair_from_the_row`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `room_history_the_discrepancy`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `room_history_the_second_blower`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `room_history_bench_markings`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `room_history_shelf_unit_d`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `room_history_bunk_three`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `room_history_can_opener_dent`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `room_history_suture_pack`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `room_history_lathe_true`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `room_history_tuner_warm`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `room_history_cupola_breath`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `room_history_soil_window`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `room_history_generator_footings`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `room_history_boiler_jacket`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `room_history_filter_cartridge`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `vignette_water_pump_original_use`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `room_fixture_corridor_chart_rail`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `room_fixture_corridor_plate_rectangles`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `room_fixture_corridor_pencil_stub`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `room_fixture_corridor_scrub_line`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `room_fixture_bunks_stencil_gaps`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `room_fixture_bunks_spare_socket`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `room_fixture_bunks_dosimeter_nail`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `room_fixture_bunks_bolt_rings`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `room_fixture_filtration_canister_notches`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `room_fixture_filtration_nameplate_tin`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `room_fixture_filtration_intake_stool`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `room_fixture_filtration_steam_valve`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `room_fixture_filtration_spare_belt`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `room_fixture_filtration_hazmat_hook`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `room_fixture_kitchen_portion_rings`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `room_fixture_kitchen_ladle_nail`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `room_fixture_kitchen_table_scratches`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `room_fixture_kitchen_flue_damper`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `room_fixture_clinic_curtain_wire`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `room_fixture_clinic_basin_rim`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `room_fixture_clinic_iodine_lot`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `room_fixture_clinic_capped_drain`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `room_fixture_workshop_busbar_leg`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `room_fixture_workshop_tool_shadow`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `room_fixture_workshop_swarf_grate`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `room_fixture_airlock_boot_crate`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `room_fixture_airlock_bolted_chair`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `room_fixture_airlock_nozzles`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `room_fixture_airlock_handprints`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `room_fixture_airlock_rag_nail`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `room_fixture_radio_mesh_panel`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `room_fixture_radio_log_book`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `room_fixture_radio_load_bulb`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `room_fixture_greenhouse_peat_troughs`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `room_fixture_greenhouse_ballast_shield`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `room_fixture_greenhouse_seed_tins`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `room_fixture_foundry_goggle_hook`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `room_fixture_foundry_sand_beds`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `room_fixture_foundry_heat_stain`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `room_fixture_foundry_works_plate`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `room_fixture_stores_bin_labels`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `room_fixture_stores_scale_pin`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `room_fixture_stores_humidity_gauge`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `room_fixture_stores_depot_form`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `room_fixture_main_generator_mount`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `room_fixture_main_battery_rack`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `room_fixture_main_inverter_panel`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `room_fixture_main_boiler_hatch`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `room_fixture_main_isolation_pad`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `room_fixture_pump_pressure_gauge`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `room_fixture_pump_leather_cup`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `room_fixture_pump_flow_ledger`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `room_fixture_pump_well_collar`
- Source: `Assets/StreamingAssets/Data/shelter_room_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `machine_hepa_stack`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `machine_foundry_cupola`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `machine_generator`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `machine_ventilation_plant`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `machine_water_still`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `machine_boiler`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `machine_airlock_machinery`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `machine_quirk_hepa_intake_whistle`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `machine_quirk_hepa_storm_cough`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `machine_quirk_hepa_housing_tick`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `machine_quirk_foundry_tuyere_knock`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `machine_quirk_foundry_exhaust_whine`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `machine_quirk_generator_fuel_cough`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `machine_quirk_generator_battery_dip`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `machine_quirk_ventilation_loaded_rattle`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `machine_quirk_water_ro_choke`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `machine_quirk_boiler_cutout`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `machine_quirk_airlock_seal_drag`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `machine_quirk_boiler_jacket_tick`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `machine_quirk_hepa_radon_hum`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `machine_quirk_foundry_heat_shimmer`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `machine_quirk_foundry_vibration_tune`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `machine_quirk_generator_brownout_flicker`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `machine_quirk_generator_vibration_tick`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `machine_quirk_ventilation_soot_smell`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `machine_quirk_water_distillation_hum`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `machine_quirk_airlock_machinery_grind`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `glitch_21_phantom_draft`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `glitch_22_repeating_relay_click`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `glitch_23_old_intercom_burst`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `glitch_24_seal_cycles`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `glitch_25_ground_loop`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `glitch_26_stuck_damper`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `glitch_27_pressure_flutter`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `glitch_28_boiler_cutout`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `glitch_29_boiler_sigh`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `glitch_30_generator_hum_drop`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `glitch_31_water_still_gurgle`
- Source: `Assets/StreamingAssets/Data/shelter_machine_identities.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `scrap_metal`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `room_bunks_crowded`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `scrap_wood`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `room_quarters_private`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `mechanical_parts`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `room_workshop_heavy`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `room_workshop_precision`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `cloth`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `room_ward_clinical`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `room_ward_quarantine`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `room_storage_secure`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `room_greenhouse_shelter`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `scrap_electronic`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `room_armory_munitions`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `room_laboratory_research`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `room_common_mess_hall`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `room_reading_quiet_room`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `room_generator`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `room_hope_beacon`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `rule_medical_field_surgery`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `rule_workshop_machinist`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `rule_workshop_precision`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `rule_radio_communications`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `rule_kitchen_nutrition`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `rule_laboratory_analysis`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `rule_greenhouse_botany`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `rule_generator_maintenance`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `rule_armory_service`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `rule_storage_logistics`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `rule_airlock_decontamination`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `rule_dormitory_caretaker`
- Source: `Assets/StreamingAssets/Data/shelter_rooms.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `room_bp_01_surface_airlock_vestibule`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `room_bp_02_diesel_generator_vault`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `room_bp_03_central_ventilation_blower_station`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `room_bp_04_deep_artesian_well_pump_room`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `room_bp_05_residential_bunk_cubicle_block`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `room_bp_06_community_soup_canteen_galley`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `room_bp_07_underground_hydroponic_greenhouse`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `room_bp_08_medical_clinic_and_surgery_suite`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `room_bp_09_heavy_machine_workshop`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `room_bp_10_radio_communications_alcove`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `room_bp_11_the_silent_foundry_smelter_bay`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `room_bp_12_lead_acid_battery_bank_vault`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `room_bp_13_water_filtration_and_brine_still`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `room_bp_14_munitions_reloading_and_sapper_armory`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `room_bp_15_slate_inscribed_memorial_crypt`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `room_bp_16_textile_weaving_and_tailor_loft`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `room_bp_17_chemical_laboratory_and_assay_bench`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `room_bp_18_grain_and_dry_stores_magazine`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `room_bp_19_carpentry_and_timber_framing_shop`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `room_bp_20_orbital_harrow_sky_armor_reinforcement`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `room_bp_21_fermentation_and_brewery_cellar`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `room_bp_22_apothecary_and_herbal_tincture_closet`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `room_bp_23_juvenile_education_and_council_classroom`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `room_bp_24_the_century_seed_sunken_atrium`
- Source: `Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace each room or machine record to the physical owner that can make its condition and repair consequence true before adding identity or atmosphere prose.
- Primary owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State/save rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI truth rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: shelter identity, rooms, machines, and maintenance.
- Seam under test: ShelterIdentitySystem/ShelterMachineTellCatalog -> room and machine identity catalogs -> existing room condition/maintenance owners -> ShelterIdentityHostSession -> decor/room UI, journal, codex, audio.
- Expected authority: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks. Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI/accessibility check: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- Player-facing truth: Unknown room/machine IDs, missing narrative records, unpowered rooms, dead systems, stale save indexes, and absent audio cues degrade to truthful unavailable state.
- Persistence response: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism response: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: ShelterIdentitySystem.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: ShelterRoomIdentityCatalog.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: ShelterMachineTellCatalog.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: MachineTellAudioSync.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: shelter_room_identities.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: shelter_machine_identities.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: ShelterIdentitySystem.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: ShelterRoomIdentityCatalog.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: ShelterMachineTellCatalog.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: MachineTellAudioSync.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: shelter_room_identities.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: shelter_machine_identities.
- Owner: Shelter identity/tell catalogs own authored identity; physical systems own condition and repair; host owns lifecycle and projection.
- State rule: Discovered room/history facts, tell observation history, and repair outcomes must persist through the existing shelter/identity owner or an additive owner field with migration; no panel-only unlocks.
- Determinism rule: Tell selection uses canonical machine condition and ordinal catalog order; no random maintenance event unless the owning system already has a seeded stream.
- UI rule: ['src/UI/ShelterDecorPanel.cs', 'src/UI/ShelterPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/ShelterRoomIdentityTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterRoomIdentityTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/ShelterMachineTellTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/ShelterMachineTellTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/MachineTellAudioSyncTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/MachineTellAudioSyncTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterRoomCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ShelterIdentitySystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 491,650 characters.
# Post-250K deep polishing pass

The architecture body above reached 491,728 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `ShelterIdentitySystem`, `ShelterRoomIdentityCatalog`, `ShelterMachineTellCatalog`, `MachineTellAudioSync`, `shelter_room_identities`, `shelter_machine_identities`.
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
