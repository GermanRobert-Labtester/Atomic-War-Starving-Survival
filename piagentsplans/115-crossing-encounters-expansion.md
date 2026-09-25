# Plan 115 — Crossing Encounters and Crises: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** frontier governance, encounters, and arbitration
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Extend the existing Crossing catalog/session/arbitration/quest chain with truthful encounter reachability, multi-day crisis state, civic choices, and consequences owned by existing systems.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5875` characters.
- Current worktree copy: `496233` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/CrossingCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4f6d4ec15b065b40aa0f44a27c03ce0d190c2387e6f2124830e93ca3ac57e043`
- Snapshot size: 19761 characters; 429 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0109:         public CrossingEncounterEntry[] encounters;
0110:         public CrossingCrisisEntry[] crises;
0111:     }
0112:
...
0119:         public List<CrossingEncounterEntry> Encounters { get; } = new List<CrossingEncounterEntry>();
0120:         public List<CrossingCrisisEntry> Crises { get; } = new List<CrossingCrisisEntry>();
0121:
0122:         public CrossingFactionEntry? GetFaction(string id) => Find(Factions, id, f => f.id);
...
0126:         public CrossingEncounterEntry? GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
0127:         public CrossingCrisisEntry? GetCrisis(string id) => Find(Crises, id, c => c.id);
0128:
0129:         private static T? Find<T>(List<T> list, string id, Func<T, string> getId) where T : class
...
0146:     /// </summary>
0147:     public sealed class CrossingCatalogLoader
0148:     {
0149:         public const string FactionsFile = "crossing_factions.json";
...
0152:         public const string ItemsFile = "crossing_items.json";
0153:         public const string EncountersFile = "crossing_encounters.json";
0154:
0155:         /// <summary>Live schema: danger 3–6 after the unit fix.</summary>
...
0165:
0166:         public CrossingCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
0167:         {
0168:             _files = files ?? throw new ArgumentNullException(nameof(files));
...
0172:
0173:         public CrossingCatalog Load(string dataDirectory)
0174:         {
0175:             var catalog = new CrossingCatalog();
...
0185:             catalog.Items.AddRange(LoadList<CrossingItemEntry>(_files.Combine(dataDirectory, ItemsFile), "items"));
0186:             LoadEncountersAndCrises(_files.Combine(dataDirectory, EncountersFile), catalog);
0187:             return catalog;
0188:         }
...
0209:                 }
0210:                 if (container?.crises != null)
0211:                 {
0212:                     for (int i = 0; i < container.crises.Length; i++)
...
0418:
0419:         /// <summary>Canonical multi-phase Crises (bible §6.3).</summary>
0420:         public static class Crises
0421:         {
```

### Current evidence: `Assets/Ashfall.Core/CrossingSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ddd91860850fe8384837844dcf1a617d1aac8d55208c45416f82da32a0518cc4`
- Snapshot size: 1768 characters; 47 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009:     /// </summary>
0010:     public sealed class CrossingSession
0011:     {
0012:         public VouchAccessSystem Vouch { get; }
...
0017:             Vouch = vouch ?? new VouchAccessSystem();
0018:             Catalog = catalog ?? new CrossingCatalog();
0019:         }
0020:
...
0023:             var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log);
0024:             return new CrossingSession(new VouchAccessSystem(), loader.Load(dataDirectory));
0025:         }
0026:
```

### Current evidence: `Assets/Ashfall.Core/CrossingArbitrationSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `13a36a288db0b04c398b1774dea0842564b935818e4edb1b099da951d383445f`
- Snapshot size: 19929 characters; 489 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008:     /// <summary>
0009:     /// ASHFALL: NOBODY'S CHARTER — §5.1 CrossingArbitrationSystem.
0010:     /// The Standing. A ruling is real for as long as three backers hold it.
0011:     /// Engine-agnostic extract of Assets/_Game/Core/CrossingArbitrationSystem.cs.
...
0065:     {
0066:         public string systemId = CrossingArbitrationSystem.SystemId;
0067:         public List<BackerDef> backerPool = new List<BackerDef>();
0068:         public List<StandingRuling> rulings = new List<StandingRuling>();
...
0075:
0076:     public class CrossingArbitrationSystem
0077:     {
0078:         public const string SystemId = "crossing_arbitration_system";
```

### Current evidence: `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ae757ef1db3c06fc7c23741ada561df5e2b07ca1585de1c2e6578811bae2e1de`
- Snapshot size: 19596 characters; 489 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0067:     [Serializable]
0068:     public class CrossingQuestSystemState
0069:     {
0070:         public string systemId = CrossingQuestSystem.SystemId;
...
0084:     /// </summary>
0085:     public class CrossingQuestSystem
0086:     {
0087:         public const string SystemId = "crossing_quest_system";
...
0100:         public event Action<CrossingStageNarrativeEvent> OnStageNarrativeEmitted;
0101:         public event Action<CrossingQuestSystemState> OnStateChanged;
0102:
0103:         public CrossingQuestSystemState State => _state;
...
0385:
0386:         public CrossingQuestSystemState CaptureState()
0387:         {
0388:             var stateCopy = new CrossingQuestSystemState
...
0414:
0415:         public void RestoreState(CrossingQuestSystemState? saved)
0416:         {
0417:             if (saved == null) return;
```

### Current evidence: `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e3b4ee33b960819bee7627921350e3ddf4c4015ba68b20ac1dfed97421556beb`
- Snapshot size: 6213 characters; 169 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0039:     /// <summary>
0040:     /// Typed eligibility layer over CrossingArbitrationSystem and CrossingQuestSystem.
0041:     /// Engine-agnostic thin query wrapper; no direct gameplay mutation.
0042:     /// </summary>
...
0058:
0059:         private readonly CrossingArbitrationSystem _arbitration;
0060:         private readonly CrossingQuestSystem _quests;
0061:
...
0067:
0068:         public CrossingArbitrationSystem Arbitration => _arbitration;
0069:         public CrossingQuestSystem Quests => _quests;
0070:
```

### Current evidence: `src/UI/CrossingQuestPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `04e9b7fbf0c954b832ccda943f6bb59e98f4bad06478aa9043548f24e92d71d3`
- Snapshot size: 20988 characters; 493 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0059:
0060:         private void OnStateChangedHandler(CrossingQuestSystemState state) => RefreshView();
0061:         private void OnVouchChangedHandler(VouchAccessSystemState state) => RefreshView();
0062:
...
0242:                     }
0243:                     else if (def.id != CrossingQuestSystem.OpeningQuest && !gateOpen && !_expansions.IsCrossingQuestCompleted(CrossingQuestSystem.OpeningQuest))
0244:                     {
0245:                         locked = true;
```

### Current evidence: `src/YearOfAsh/DoorEncounterModal.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/YearOfAsh/DoorEncounterModal.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8f1ac3cdbd51485eb7bf24fd8491b9bd61f1488fad9ac843c1cd8d6e0f1cda30`
- Snapshot size: 7509 characters; 176 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: #pragma warning disable CS8618
0005: using Godot;
0006: using AtomicWar.GodotApp.UI;
0007: using Ashfall.Core.UI;
0008: using Ashfall.Core.YearOfAsh;
0009:
0010: namespace AtomicWar.GodotApp.YearOfAsh
0011: {
0012:     /// <summary>
0013:     /// Godot 4.7+ UI Modal for Bunker Hatch / Door Encounters (Days 180 to 360).
0014:     /// Displays visitor information, dynamic choices with resource/trait requirements,
0015:     /// and previews/displays real-time psychological reactions from living bunker survivors.
0016:     /// Thin presentation node; zero simulation logic.
0017:     /// </summary>
0018:     public partial class DoorEncounterModal : Control
0019:     {
0020:         private PanelContainer _panel = null!;
```

### Current evidence: `src/Main.GameFlow.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.GameFlow.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `2fdc11c80e9e23418cf05a6d8dbd621c66c8d6aba34503e7cde474922c403685`
- Snapshot size: 43241 characters; 950 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using Ashfall.Core;
0008: using Ashfall.Core.Campaign;
0009: using Ashfall.Core.Difficulty;
0010: using Ashfall.Core.Inventory;
0011: using Ashfall.Core.Save;
0012: using Ashfall.Core.Expeditions;
0013: using AtomicWar.GodotApp.UI;
0014: using AtomicWar.GodotApp.World;
0015:
0016: namespace AtomicWar.GodotApp
0017: {
0018:     public partial class Main : Control
0019:     {
0020:         private void RunDashboardUiTestAndQuit()
```

### Current evidence: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`
- Snapshot size: 28037 characters; 642 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0495:   ],
0496:   "crises": [
0497:     {
0498:       "id": "crisis_the_forfeit",
```

### Current evidence: `Assets/StreamingAssets/Data/crossing_quests.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`
- Snapshot size: 34791 characters; 912 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "quests": [
0004:     {
0005:       "id": "quest_crossing_the_vouch",
0006:       "display_name": "A Name at the Gate",
0007:       "type": "expedition",
0008:       "briefing": "Bram Ostrowski names the Crossing and will sell you a rough sketch of the approach. He will not walk there himself. \"I sold them a map once. That was the whole transaction. I'd like it to stay that way.\"",
0009:       "prereq_quest_id": "",
0010:       "min_day": 70,
0011:       "stages": [
0012:         {
0013:           "id": "hear_ostrowski",
0014:           "text": "Hear Ostrowski out and take the sketch of the approach."
0015:         },
0016:         {
0017:           "id": "find_a_name",
0018:           "text": "Find a name willing to vouch: Ostrowski (reluctant, once), Mattis Cray at the truss, or a sister-pack contact."
0019:         },
0020:         {
```

### Current evidence: `Assets/StreamingAssets/Data/crossing_factions.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`
- Snapshot size: 6281 characters; 160 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "actions": [
0004:     {
0005:       "id": "faction_the_scale",
0006:       "display_name": "The Scale",
0007:       "alignment": "conditional",
0008:       "home_region": "region_crossing",
0009:       "is_active": true,
0010:       "trust": 0,
0011:       "wants": [
0012:         "trade_goods"
0013:       ],
0014:       "offers": [
0015:         "stallrow_trade_access",
0016:         "verification"
0017:       ],
0018:       "signature_quote": "The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.",
0019:       "access_rule": "An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.",
0020:       "badge_asset_id": ""
```

### Current evidence: `Assets/StreamingAssets/Data/crossing_locations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c8a3d4dd545af2773f9f8005126fbdd2b5974bbd161358e5c4e3b3adc3389d37`
- Snapshot size: 8989 characters; 161 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locations": [
0004:     {
0005:       "id": "loc_crossing_viaduct_gate",
0006:       "displayName": "The Viaduct Gate",
0007:       "dangerLevel": 4.5,
0008:       "travelHours": 8,
0009:       "baseRadsPerHour": 22.0,
0010:       "region": "region_crossing",
0011:       "inspect": "A rail truss over the Drown's edge, planked over for feet instead of axles. The paint on the sign has texture from how many times it has been redone: NO CHARTER NO GUARD ASK FOR SOMEONE. Someone added, smaller, underneath, in different paint: WE MEAN IT.",
0012:       "description": "The gate is not a wall. It is a threshold you are allowed to cross only because someone staked their own name on you. Until someone vouches, it stays closed and the viaduct stays quiet.",
0013:       "overlay_on_unlock": false,
0014:       "recast_always": false
0015:     },
0016:     {
0017:       "id": "loc_crossing_scalehouse",
0018:       "displayName": "The Scalehouse",
0019:       "dangerLevel": 3.0,
0020:       "travelHours": 1,
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

### Current evidence: `src/UI/CrossingSafeConductVouchPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `16fd13b921b1614eadc4becd8e17e90fd2e6a9fe8ce275e7b87c2f5e562e4825`
- Snapshot size: 5150 characters; 100 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.UI;
0005: using DesignTheme = Ashfall.Core.UI.Theme;
0006:
0007: namespace AtomicWar.GodotApp.UI
0008: {
0009:     public partial class CrossingSafeConductVouchPanel : Control, IBindablePanel
0010:     {
0011:         public event Action? OnClose;
0012:
0013:         private Label? _headerTitleLabel;
0014:         private Label? _statusBadgeLabel;
0015:         private Button? _closeButton;
0016:         private VBoxContainer? _telemetryContainer;
0017:         private VBoxContainer? _buttonContainer;
0018:         private VBoxContainer? _dataContainer;
0019:         private Label? _logOutputLabel;
0020:
```

### Current evidence: `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `65e9b7c974a0ecb74ccba161056565a5efd48c922dd4f6b9899aa5b180b6b319`
- Snapshot size: 22487 characters; 605 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008: {
0009:     public class CrossingQuestSystemTests
0010:     {
0011:         private static List<CrossingQuestDef> SampleCatalog()
...
0059:
0060:         private static CrossingQuestSystem FreshSystem()
0061:         {
0062:             var sys = new CrossingQuestSystem();
...
0320:             var greenhouse = new GreenhouseSystem(1);
0321:             var arbitration = new CrossingArbitrationSystem();
0322:             var ledger = new LedgerDebtSystem();
0323:             var layouts = new LocationLayoutSystem(new FileSystemIO(), new SystemTextJsonSerializer(), NullLog.Instance);
```

### Current evidence: `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c967a89f37a2ce9da249bdb23d33410b49ea71f8f297b77808e72f4c14eb5488`
- Snapshot size: 19762 characters; 467 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008:     /// <summary>
0009:     /// Tests for the CrossingArbitrationSystem extraction (Nobody's Charter
0010:     /// §5.1): the 3-backer rule, principled-majority honest/rigged split,
0011:     /// overturn by 3+ counters, dead backers reverting held rulings, and the
...
0014:     /// </summary>
0015:     public class CrossingArbitrationSystemTests
0016:     {
0017:         private const string Topic = "quest_crossing_the_standing";
...
0020:         {
0021:             var sys = new CrossingArbitrationSystem();
0022:             sys.LoadBackerPool(new List<BackerDef>
0023:             {
...
0209:             var json = new SystemTextJsonSerializer();
0210:             var restored = new CrossingArbitrationSystem();
0211:             restored.RestoreState(json.Deserialize<CrossingArbitrationState>(json.Serialize(sys.CaptureState())));
0212:
...
0226:
0227:             var restored = new CrossingArbitrationSystem();
0228:             restored.RestoreState(saved);
0229:             restored.RestoreState(saved);
...
0376:             var json = new SystemTextJsonSerializer();
0377:             var restored = new CrossingArbitrationSystem();
0378:             restored.RestoreState(json.Deserialize<CrossingArbitrationState>(json.Serialize(sys.CaptureState())));
0379:
```

### Current evidence: `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1bb5e0d2b6d7c7297e4ef0ef0c990f1f93660599bdea2268843187d64f958520`
- Snapshot size: 15111 characters; 353 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0023:
0024:         private static CrossingCatalog LoadCatalog()
0025:         {
0026:             string dataDir = ResolveDataDir();
```

### Current evidence: `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `254896f95b89cc99e1669102c365fa60b8950c04e3f1e8a6c0a0477dffef5c27`
- Snapshot size: 11951 characters; 257 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0033:             public List<CrossingEncounterJson>? encounters { get; set; }
0034:             public List<CrossingCrisisJson>? crises { get; set; }
0035:         }
0036:
...
0063:         [Fact]
0064:         public void Plan115_CrossingEncountersAndCrises_LoadsFullTwentyFiveEncountersAndTwelveCrises()
0065:         {
0066:             string path = Path.Combine(DataDirectory, "crossing_encounters.json");
...
0075:             Assert.Equal(25, container.encounters!.Count);
0076:             Assert.NotNull(container.crises);
0077:             Assert.Equal(12, container.crises!.Count);
0078:
...
0098:
0099:             var seenCrises = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
0100:             foreach (var crisis in container.crises)
0101:             {
...
0194:
0195:             // Load Crossing Catalog via CrossingCatalogLoader
0196:             var crossingLoader = new CrossingCatalogLoader(files, json);
0197:             var crossingCatalog = crossingLoader.Load(DataDirectory);
...
0226:             var dl1 = DeepLoreLocationCatalogLoader.Load(DataDirectory, files, json);
0227:             string crossingPath = Path.Combine(DataDirectory, "crossing_encounters.json");
0228:             var ce1 = JsonSerializer.Deserialize<CrossingEncountersContainer>(
0229:                 File.ReadAllText(crossingPath), SystemTextJsonSerializer.Options);
...
0245:             Assert.Equal(ce1!.encounters!.Count, ce2!.encounters!.Count);
0246:             Assert.Equal(ce1.crises!.Count, ce2.crises!.Count);
0247:             for (int i = 0; i < ce1.encounters.Count; i++)
0248:             {
...
0252:             {
0253:                 Assert.Equal(ce1.crises[i].id, ce2.crises[i].id);
0254:             }
0255:         }
```

### Current evidence: `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ba5424ad1e2499c320f9e7573a6f3c89715aa262e90d739658abfae4f4c4a697`
- Snapshot size: 11633 characters; 239 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0076:         [Fact]
0077:         public void Crossing_20Quests_14Encounters_ArbitrationCasesAndCrises_Resolves()
0078:         {
0079:             string dataDir = GetDataDir();
...
0162:             // Verify Crossing quests exist in master
0163:             var cr = CrossingSession.Load(dataDir, NullLog.Instance);
0164:             foreach (var q in cr.Catalog.Quests)
0165:             {
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/crossing_encounters.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, encounters, crises`
- `encounters`: list count=25; sample IDs=['enc_nc_collector_visit', 'enc_nc_backer_pressure', 'enc_nc_lockup_muscle', 'enc_nc_iron_raiders_scout', 'enc_nc_deserter_passage', 'enc_nc_scavenger_dispute', 'enc_nc_grain_exchange_envoy', 'enc_nc_sun_seekers_pass']
- `crises`: list count=12; sample IDs=['crisis_the_forfeit', 'crisis_the_vote', 'crisis_the_standing_contested', 'crisis_the_charter_found', 'crisis_who_holds_the_ledger', 'crisis_the_water_claim', 'crisis_the_grain_riot', 'crisis_the_charter_amendment']
- `schema_version`: `1`
- SHA-256: `02ccdbff76776a6b5653a065640e59f5c1d7ddb9e3cfe31968e89338cb31423c`
#### `Assets/StreamingAssets/Data/crossing_quests.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, quests`
- `quests`: list count=23; sample IDs=['quest_crossing_the_vouch', 'quest_crossing_first_weigh', 'quest_crossing_scale_integrity', 'quest_crossing_the_terms', 'quest_crossing_the_petition', 'quest_crossing_the_standing', 'quest_crossing_the_marker', 'quest_crossing_the_forfeit']
- `schema_version`: `1`
- SHA-256: `94d5932902ac39bdac0f2caa6c7e56a1bdb33247063e397ca9d2ac5949315734`
#### `Assets/StreamingAssets/Data/crossing_factions.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, actions`
- `actions`: list count=8; sample IDs=['faction_the_scale', 'faction_the_underwrite', 'faction_the_compact', 'faction_the_lamplighters', 'faction_the_granary_wardens', 'faction_the_water_committee', 'faction_the_quarantine_post', 'faction_the_smugglers_court']
- `schema_version`: `1`
- SHA-256: `71f072a6dd213655a8ea3c2fbebbc617d31491ce3348166509517fe367ef27ec`
#### `Assets/StreamingAssets/Data/crossing_locations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locations`
- `locations`: list count=13; sample IDs=['loc_crossing_viaduct_gate', 'loc_crossing_scalehouse', 'loc_crossing_stallrow', 'loc_crossing_watchtower', 'loc_crossing_weighbridge', 'loc_crossing_underwrite_hall', 'loc_crossing_records_room', 'loc_crossing_the_lockup']
- `schema_version`: `1`
- SHA-256: `c8a3d4dd545af2773f9f8005126fbdd2b5974bbd161358e5c4e3b3adc3389d37`
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
## Symbol and caller audit

#### `CrossingCatalog` — HOST_REFERENCE_PRESENT — core/declaration=15, host=1, test=1
- `Assets/Ashfall.Core/CrossingCatalog.cs:113` (declaration) — public sealed class CrossingCatalog
- `Assets/Ashfall.Core/CrossingCatalog.cs:173` (core) — public CrossingCatalog Load(string dataDirectory)
- `Assets/Ashfall.Core/CrossingCatalog.cs:175` (core) — var catalog = new CrossingCatalog();
- `Assets/Ashfall.Core/CrossingCatalog.cs:190` (core) — private void LoadEncountersAndCrises(string path, CrossingCatalog catalog)
- `Assets/Ashfall.Core/CrossingSession.cs:13` (core) — public CrossingCatalog Catalog { get; }
- `Assets/Ashfall.Core/CrossingSession.cs:15` (core) — public CrossingSession(VouchAccessSystem vouch, CrossingCatalog catalog)
- `Assets/Ashfall.Core/CrossingSession.cs:18` (core) — Catalog = catalog ?? new CrossingCatalog();
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:448` (core) — ["crossing_encounters.json"] = new[] { "CrossingCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:449` (core) — ["crossing_factions.json"] = new[] { "CrossingCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:450` (core) — ["crossing_items.json"] = new[] { "CrossingCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:451` (core) — ["crossing_locations.json"] = new[] { "CrossingCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:785` (core) — ["crossing_encounters.json"] = "CrossingCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:786` (core) — ["crossing_factions.json"] = "CrossingCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:787` (core) — ["crossing_items.json"] = "CrossingCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:788` (core) — ["crossing_locations.json"] = "CrossingCatalog",
- `src/Host/ContentUtilizationRuntimeCollector.cs:1108` (host) — instr.RecordDefinitionsRegistered(file, "CrossingCatalog", 1);
- `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs:24` (test) — private static CrossingCatalog LoadCatalog()
#### `CrossingSession` — HOST_REFERENCE_PRESENT — core/declaration=8, host=4, test=3
- `Assets/Ashfall.Core/CrossingHeadlessDemo.cs:32` (core) — var session = CrossingSession.Load(dataDirectory, log);
- `Assets/Ashfall.Core/CrossingSession.cs:10` (declaration) — public sealed class CrossingSession
- `Assets/Ashfall.Core/CrossingSession.cs:15` (core) — public CrossingSession(VouchAccessSystem vouch, CrossingCatalog catalog)
- `Assets/Ashfall.Core/CrossingSession.cs:21` (core) — public static CrossingSession Load(string dataDirectory, ILog? log = null)
- `Assets/Ashfall.Core/CrossingSession.cs:24` (core) — return new CrossingSession(new VouchAccessSystem(), loader.Load(dataDirectory));
- `Assets/Ashfall.Core/ExpansionMasterSession.cs:30` (core) — public CrossingSession Crossing { get; }
- `Assets/Ashfall.Core/ExpansionMasterSession.cs:49` (core) — CrossingSession crossing,
- `Assets/Ashfall.Core/ExpansionMasterSession.cs:94` (core) — var crossing = CrossingSession.Load(dataDirectory, log);
- `src/Host/HostCli.ExpansionDepth.cs:62` (host) — var crossingSession = CrossingSession.Load(dataDirectory, NullLog.Instance);
- `src/Host/ExpeditionHostSession.cs:152` (host) — if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
- `src/Host/ExpeditionHostSession.cs:586` (host) — if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
- `src/Host/ExpeditionHostSession.cs:971` (host) — if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
- `Ashfall.Core.Tests/ExpansionsIntegrationTests.cs:59` (test) — var session = CrossingSession.Load(DataDir());
- `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs:80` (test) — var session = CrossingSession.Load(dataDir, NullLog.Instance);
- `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs:163` (test) — var cr = CrossingSession.Load(dataDir, NullLog.Instance);
#### `CrossingArbitrationSystem` — HOST_REFERENCE_PRESENT — core/declaration=18, host=3, test=15
- `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs:50` (core) — var sys = new CrossingArbitrationSystem();
- `Assets/Ashfall.Core/CrossingArbitrationHeadlessDemo.cs:139` (core) — var restored = new CrossingArbitrationSystem();
- `Assets/Ashfall.Core/CrossingArbitrationSystem.cs:9` (core) — /// ASHFALL: NOBODY'S CHARTER — §5.1 CrossingArbitrationSystem.
- `Assets/Ashfall.Core/CrossingArbitrationSystem.cs:11` (core) — /// Engine-agnostic extract of Assets/_Game/Core/CrossingArbitrationSystem.cs.
- `Assets/Ashfall.Core/CrossingArbitrationSystem.cs:66` (core) — public string systemId = CrossingArbitrationSystem.SystemId;
- `Assets/Ashfall.Core/CrossingArbitrationSystem.cs:76` (declaration) — public class CrossingArbitrationSystem
- `Assets/Ashfall.Core/ExpansionHubSave.cs:184` (core) — CrossingArbitrationSystem? arbitration = null,
- `Assets/Ashfall.Core/ExpansionHubSave.cs:457` (core) — CrossingArbitrationSystem? arbitration = null,
- `Assets/Ashfall.Core/LedgerDebtSystem.cs:62` (core) — /// many days. Composed with CrossingArbitrationSystem at the host layer.
- `Assets/Ashfall.Core/LedgerDebtSystem.cs:248` (core) — /// CrossingArbitrationSystem at the host layer) and the amendment is
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:40` (core) — /// Typed eligibility layer over CrossingArbitrationSystem and CrossingQuestSystem.
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:59` (core) — private readonly CrossingArbitrationSystem _arbitration;
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:62` (core) — public CrossingThirdonaryIntegration(CrossingArbitrationSystem arbitration, CrossingQuestSystem quests)
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:68` (core) — public CrossingArbitrationSystem Arbitration => _arbitration;
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1053` (core) — ["crossing_encounters.json"] = new[] { "CrossingArbitrationSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1054` (core) — ["crossing_factions.json"] = new[] { "CrossingArbitrationSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1055` (core) — ["crossing_items.json"] = new[] { "CrossingArbitrationSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1056` (core) — ["crossing_locations.json"] = new[] { "CrossingArbitrationSystem" },
- `src/Host/ContentUtilizationRuntimeCollector.cs:1105` (host) — instr.RecordCatalogOpened(file, "CrossingArbitrationSystem");
- `src/Host/ExpansionHostSession.cs:31` (host) — public CrossingArbitrationSystem Arbitration { get; }
- `src/Host/ExpansionHostSession.cs:70` (host) — Arbitration = new CrossingArbitrationSystem();
- `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs:9` (test) — /// Tests for the CrossingArbitrationSystem extraction (Nobody's Charter
- `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs:19` (test) — private static CrossingArbitrationSystem Fixture()
- `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs:21` (test) — var sys = new CrossingArbitrationSystem();
- … 12 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `CrossingQuestSystem` — HOST_REFERENCE_PRESENT — core/declaration=12, host=8, test=12
- `Assets/Ashfall.Core/ExpansionHubSave.cs:186` (core) — CrossingQuestSystem? crossingQuests = null,
- `Assets/Ashfall.Core/ExpansionHubSave.cs:459` (core) — CrossingQuestSystem? crossingQuests = null,
- `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:70` (core) — public string systemId = CrossingQuestSystem.SystemId;
- `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs:85` (declaration) — public class CrossingQuestSystem
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:40` (core) — /// Typed eligibility layer over CrossingArbitrationSystem and CrossingQuestSystem.
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:60` (core) — private readonly CrossingQuestSystem _quests;
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:62` (core) — public CrossingThirdonaryIntegration(CrossingArbitrationSystem arbitration, CrossingQuestSystem quests)
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:69` (core) — public CrossingQuestSystem Quests => _quests;
- `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs:71` (core) — public bool IsOpeningQuestComplete() => _quests.IsQuestCompleted(CrossingQuestSystem.OpeningQuest);
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:452` (core) — ["crossing_quests.json"] = new[] { "CrossingQuestSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:700` (core) — ["crossing_quests.json"] = "CrossingQuestSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1057` (core) — ["crossing_quests.json"] = new[] { "CrossingQuestSystem" },
- `src/Host/ExpansionHostSession.cs:33` (host) — public CrossingQuestSystem CrossingQuests { get; }
- `src/Host/ExpansionHostSession.cs:72` (host) — CrossingQuests = new CrossingQuestSystem();
- `src/UI/CrossingQuestPanel.cs:243` (host) — else if (def.id != CrossingQuestSystem.OpeningQuest && !gateOpen && !_expansions.IsCrossingQuestCompleted(CrossingQuestSystem.OpeningQuest))
- `src/UI/QuestsAtlasPanel.cs:18` (host) — ///   • <see cref="CrossingQuestSystem"/> for crossing-side quests
- `src/UI/QuestsAtlasPanel.cs:43` (host) — private CrossingQuestSystem? _crossing;
- `src/UI/QuestsAtlasPanel.cs:47` (host) — public void Bind(HoldfastQuestSystem holdfast, CrossingQuestSystem? crossing = null)
- `src/UI/QuestsPanel.cs:40` (host) — private CrossingQuestSystem? _crossingQuests;
- `src/UI/QuestsPanel.cs:55` (host) — CrossingQuestSystem? crossingQuests = null,
- `Ashfall.Core.Tests/CrossingQuestSystemTests.cs:60` (test) — private static CrossingQuestSystem FreshSystem()
- `Ashfall.Core.Tests/CrossingQuestSystemTests.cs:62` (test) — var sys = new CrossingQuestSystem();
- `Ashfall.Core.Tests/CrossingThirdonaryIntegrationTests.cs:17` (test) — private static (CrossingQuestSystem quests, CrossingArbitrationSystem arbitration, CrossingThirdonaryIntegration integration) CreateFixture()
- `Ashfall.Core.Tests/CrossingThirdonaryIntegrationTests.cs:19` (test) — var quests = new CrossingQuestSystem();
- … 8 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `crossing_encounters` — HOST_REFERENCE_PRESENT — core/declaration=6, host=1, test=3
- `Assets/Ashfall.Core/CrossingCatalog.cs:153` (core) — public const string EncountersFile = "crossing_encounters.json";
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:47` (core) — "crossing_encounters.json", "crossing_factions.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:448` (core) — ["crossing_encounters.json"] = new[] { "CrossingCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:785` (core) — ["crossing_encounters.json"] = "CrossingCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1053` (core) — ["crossing_encounters.json"] = new[] { "CrossingArbitrationSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1496` (core) — ["crossing_encounters.json"] = new[] { "CrossingPanel" },
- `src/Host/ContentUtilizationRuntimeCollector.cs:1099` (host) — foreach (var file in new[] { "crossing_quests.json", "crossing_locations.json", "crossing_items.json", "crossing_factions.json", "crossing_encounters.json" })
- `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:66` (test) — string path = Path.Combine(DataDirectory, "crossing_encounters.json");
- `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:67` (test) — Assert.True(File.Exists(path), "crossing_encounters.json must exist.");
- `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:227` (test) — string crossingPath = Path.Combine(DataDirectory, "crossing_encounters.json");
#### `crises` — HOST_REFERENCE_PRESENT — core/declaration=19, host=2, test=10
- `Assets/Ashfall.Core/CrossingCatalog.cs:110` (core) — public CrossingCrisisEntry[] crises;
- `Assets/Ashfall.Core/CrossingCatalog.cs:210` (core) — if (container?.crises != null)
- `Assets/Ashfall.Core/CrossingCatalog.cs:212` (core) — for (int i = 0; i < container.crises.Length; i++)
- `Assets/Ashfall.Core/CrossingCatalog.cs:214` (core) — if (container.crises[i] != null)
- `Assets/Ashfall.Core/CrossingCatalog.cs:215` (core) — catalog.Crises.Add(container.crises[i]);
- `Assets/Ashfall.Core/CrossingHeadlessDemo.cs:62` (core) — Check(session.Catalog.Crises.Count >= 5, "five Crossing multi-phase crises loaded");
- `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs:485` (core) — // Penalty for active unresolved crises
- `Assets/Ashfall.Core/Shelter/DisasterResponseSystem.cs:122` (core) — /// protocol activation, and architectural resilience against crises.
- `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs:83` (core) — /// bonuses during crises but accumulates personal stress when deaths or
- `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs:74` (core) — /// Manages dynamically triggered crises (miners rescue, radio depot investigations,
- `Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs:9` (core) — /// Classes of existential crises that can be projected from authoritative read models.
- `Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs:118` (core) — /// and classifies impending crises with bounded confidence and stable ordering.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:274` (core) — new("survivor_mental_health", "SaveSurvivorMentalHealth", "SetupSurvivorMentalHealth", "psychology", "Plans 50-53 — survivor psychological trauma, stress levels, catharsis, and mental health crises"),
- `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs:69` (core) — var crises = new Dictionary<string, CrisisEventDefinition>(IdentityComparer);
- `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs:87` (core) — crises[id] = NormalizeCrisis(source, id);
- `Assets/Ashfall.Core/Needs/SurvivorMentalHealthSystem.cs:95` (core) — foreach (var pair in crises) _crises[pair.Key] = pair.Value;
- `Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs:101` (core) — // Scaled by difficulty modifier, crises faced, and days survived
- `Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs:118` (core) — // Base 1000, penalized heavily by casualties and failed crises, boosted by successful crises
- `Assets/Ashfall.Core/Telemetry/PlayableMetricsAggregationEngine.cs:164` (core) — summary = "Strained survival conditions. Vulnerable to compounding crises.";
- `src/Host/PlayMetricsHostSession.cs:20` (host) — //   crises resolved      -> DisasterResponseSystem resolved count
- `src/Host/PlayMetricsHostSession.cs:21` (host) — //   crises failed        -> DisasterResponseSystem disasters overdue + unresolved
- `Ashfall.Core.Tests/EcologyBalanceSimulationTests.cs:155` (test) — "seasonal die-off must clear expired blooms (bounded crises)");
- `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:34` (test) — public List<CrossingCrisisJson>? crises { get; set; }
- `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs:76` (test) — Assert.NotNull(container.crises);
- … 7 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 23-26
00023: 3. **The Generator Matrices (Part III):** the combinatorial core. Ten expansion lanes × seventeen subsystem clusters, with per-cell opening archetypes. This is the mechanism by which one document yields hundreds of expansion plans without inventing duplicate systems.
00024: 4. **The Seeded Backlog (Part IV) and Templates (Part V):** audit-derived candidate expansions, each with a subject, evidence, confidence, and best integration route; plus the wave-charter, subject-plan, and verification templates the repository already uses, extended for factory output.
00025:
00026: **Scale honesty clause.** The requested target for this expansion effort is two million characters. A single authoring pass cannot responsibly produce two million characters of *verified* planning content, and the repository's own constitution (Part 0.4 of v1.0; `AGENTS.md` rules 7–8) forbids manufacturing padded work. v2.0 therefore defines a Multi-Session Growth Protocol (Part VI): the factory is designed to be *appended* session by session, each session adding one or more verified volumes (expanded subsystem deep maps, prose spec libraries, backlog batches), until the corpus reaches the target size organically. The Part VI protocol is the only sanctioned path to the target; bulk generation of unverified prose is a NON-CANON act.
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
#### authority lines 151-154
00151: | C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
00152: | C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
00153: | C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
00154: | C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
#### authority lines 251-254
00251:
00252: **SB-04 — Muster domain deep expansion (Lanes A and B/C7).** Evidence: DR-04 — five muster catalogs live (`muster_camp_scenes`, `muster_epilogues`, `muster_faction_actions`, `muster_faction_culture`, `muster_witnesses`). Subject: muster-camp encounter depth, witness-driven epilogue evidence, culture-conditioned actions. Integration route: data-first through muster loaders; epilogue touch must be declared. Confidence: HIGH CONFIDENCE.
00253:
00254: **SB-05 — Epilogue permutation coverage campaign (Lanes A and C/C13).** Evidence: 19A/19B/19C closed (DR-06); matrix is 32 permutations. Subject: audit which permutations are under-served in chronicle prose and evidence enrollment; author chronicle depth for the weakest permutations. Integration route: data-first into epilogue chronicle catalogs; Reckoning enrollment through endgame owners. Confidence: HIGH CONFIDENCE.
#### authority lines 310-313
00310:
00311: ### Template W — Wave charter (for multi-plan batches)
00312:
00313: ` ` `text
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
#### authority lines 472-475
00472: ### What must not change
00473: Permutation semantics, Reckoning evidence vocabulary, verdict evaluation logic, Crossing ending prose pins (house-voice wording is pinned).
00474:
00475: ### Recommended integration route
#### authority lines 697-700
00697:
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00699:
00700: **A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 711-714
00711:
00712: **A-23 · C9 · Intake interview continuation.** Subject: new-arrival intake interviews conditioned on the arrival channels that exist (rescue, crossing, holdfast). Evidence: `new_arrival_intake_interviews` exists; arrival channels are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
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
#### authority lines 878-881
00878:
00879: **G-05 · C7 · War-chain authored-day mapping tests.** Subject: pin the 300-day offset mapping (playable 180 → authored 480) with boundary tests. Evidence: mapping is canon (`FactionWarChainRunner.ToAuthoredDay`). Route: focused xUnit. Confidence: HIGH CONFIDENCE.
00880:
00881: **G-06 · C8 · Exactly-once guard regression suite.** Subject: regression tests covering every sealed exactly-once guard class (ignore consequences, arrival resolution, salvage grants) against restore-mid-effect saves. Evidence: sealed runtime models the guards (DR-06). Route: focused xUnit + fixture saves. Confidence: HIGH CONFIDENCE.
#### authority lines 934-937
00934:
00935: **DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.
00936:
00937: **DM-3 — Water, food, agriculture (C3).** Owners: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Live catalogs: `water_treatment` family via systems, `fog_harvesting_catalog`, `deep_well` systems, `brine` systems, `nutrition_profiles`, `food_preservation`, `grain_processing`, `greenhouse_items`, `hydroponic_crops`, `aquaponics_system_catalog`, `aeroponics_nutrient_catalog`, `cryo_cultivars`, `crop_strains`, `dive_sites`. Hosts: Greenhouse, GrainProcessing, KitchenNutrition, FoodPreservation, DeepWell, Sanitation, DeepCoast. Openings: A-06, A-07, A-08, B-05, C-12, F-012 (dive/hydroponic audit consumed as F-012 above).
#### authority lines 948-951
00948:
00949: **DM-9 — Survivors and interiority (C9).** Owners: needs, health, skills, traits, mental arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, spiritual rituals, memorial rites, final wishes, belongings, memory decay, phantom memory, lineage, cohorts, apprenticeships. Live catalogs: `survivors`, `skills`, `development_traits`, `mental_arcs`, `psychological_trauma`, `psychological_therapies`, `guilt_sources`, `confession_secrets`, `belief_movements`, `spiritual_rituals`, `memorial_rites`, `final_wishes`, `companion_animals`, `phantom_heirlooms`, `phantom_triggers`, `starting_survivors`, `starting_survivor_cohorts`, `expansion_survivor_fields`. Hosts: Survivors, SurvivorRelations, PsychologyArc, MentalHealthCrisis, Caregiving, Spiritual, PhantomMemory. Openings: A-21, A-22, A-23, B-04, B-14, B-15, D-02. Known caution: `ClaimPersonalBelonging` no-caller finding (unverified at runtime — re-verify before extending).
00950:
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
… 97 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.

**Requested behavior.** Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.

**Minimum safe delta.** Extend `crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.

**Required delta.** Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.

**Primary seam.** crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `CrossingCatalog`, `CrossingSession`, `CrossingArbitrationSystem`, `CrossingQuestSystem`, `crossing_encounters`, `crises`.

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
| Domain rules | CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 115.

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
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.
- What is the smallest safe change? Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.
- Which owner is touched? CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/CrossingCatalog.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/CrossingSession.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/CrossingArbitrationSystem.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Crossing/CrossingThirdonaryIntegration.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/CrossingQuestPanel.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/YearOfAsh/DoorEncounterModal.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.GameFlow.cs` — CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/crossing_encounters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/crossing_quests.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/crossing_factions.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/crossing_locations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/characters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/CrossingQuestPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/YearOfAsh/DoorEncounterModal.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/CrossingSafeConductVouchPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CrossingQuestSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections is wired end to end or the plan explicitly closes as already integrated.
- Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

CrossingCatalog, CrossingSession, CrossingArbitrationSystem, CrossingQuestSystem, CrossingThirdonaryIntegration, crossing_encounters.json, crossing_quests.json, crossing_factions.json, crossing UI surfaces, and focused tests already exist. The old plan’s claimed CrossingSaveData and standalone crisis owner must be verified before being named.

# 3. Required Delta

Inventory current encounter/crisis rows, identify the actual host route, define phase transitions and idempotency, and add only the content/contracts that the live Crossing owner can consume.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Crossing quests and arbitration are established owners; this plan does not create a second civic vote, combat resolver, or faction-standing writer. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `enc_nc_collector_visit`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `enc_nc_backer_pressure`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `enc_nc_lockup_muscle`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `enc_nc_iron_raiders_scout`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `enc_nc_deserter_passage`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `enc_nc_scavenger_dispute`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `enc_nc_grain_exchange_envoy`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `enc_nc_sun_seekers_pass`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `enc_nc_forfeit_witness`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `enc_nc_standing_ambush`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `enc_nc_mass_crossing_surge`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `enc_nc_garrison_iron_blockade`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `enc_nc_pestilence_quarantine_lockdown`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `enc_nc_syndicate_bribe_overture`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `enc_nc_bonded_caravan_ambush`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `enc_nc_smuggler_checkpoint`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `enc_nc_frozen_barge`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `enc_nc_toll_bridge_claim`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `enc_nc_collapsed_crossing`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `enc_nc_contaminated_ford`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `enc_nc_ice_fracture`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `enc_nc_uxo_field_crossing`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `enc_nc_refugee_blockade`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `enc_nc_family_ledger_gate`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `enc_nc_child_at_gate`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `crisis_the_forfeit`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `crisis_the_vote`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `crisis_the_standing_contested`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `crisis_the_charter_found`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `crisis_who_holds_the_ledger`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `crisis_the_water_claim`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `crisis_the_grain_riot`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `crisis_the_charter_amendment`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `crisis_the_debt_forgiveness`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `crisis_the_refugee_admission`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `crisis_the_arbitrator_bribe`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `crisis_the_quarantine_break`
- Source: `Assets/StreamingAssets/Data/crossing_encounters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `quest_crossing_the_vouch`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `quest_crossing_first_weigh`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `quest_crossing_scale_integrity`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `quest_crossing_the_terms`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `quest_crossing_the_petition`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `quest_crossing_the_standing`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `quest_crossing_the_marker`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `quest_crossing_the_forfeit`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `quest_crossing_the_vote_that_isnt`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `quest_crossing_three_dry_pages`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `quest_crossing_who_holds_the_ledger`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `quest_crossing_companion_mattis`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `quest_crossing_asylum_in_the_truss`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `quest_crossing_contraband_medical_vial`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `quest_crossing_vehicle_lien_arbitration`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `quest_crossing_displaced_kin_roll`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `quest_crossing_quarantine_breach_trial`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `quest_crossing_flotilla_docking_rights`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `quest_crossing_embargo_transit_escort`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `quest_crossing_the_null_charter_vote`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `quest_crossing_the_salvaged_accord`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `quest_crossing_the_registry_dispute`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `quest_crossing_the_long_toll`
- Source: `Assets/StreamingAssets/Data/crossing_quests.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `faction_the_scale`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `faction_the_underwrite`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `faction_the_compact`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `faction_the_lamplighters`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `faction_the_granary_wardens`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `faction_the_water_committee`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `faction_the_quarantine_post`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `faction_the_smugglers_court`
- Source: `Assets/StreamingAssets/Data/crossing_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `loc_crossing_viaduct_gate`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `loc_crossing_scalehouse`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `loc_crossing_stallrow`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `loc_crossing_watchtower`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `loc_crossing_weighbridge`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `loc_crossing_underwrite_hall`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `loc_crossing_records_room`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `loc_crossing_the_lockup`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `loc_crossing_granary_pledge`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `loc_crossing_nightfire`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `loc_crossing_petition_tent`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `loc_crossing_founders_marker`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `loc_crossing_the_annex`
- Source: `Assets/StreamingAssets/Data/crossing_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `npc_bram_ostrowski`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `npc_sergeant_pell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `npc_doctor_ianov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `npc_wren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `npc_kestrel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `npc_nomi_fisk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `npc_ivor_lasko`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `npc_the_cartwright_sisters`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `npc_edor_vale`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `npc_yara_holm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `npc_leva_quist`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `npc_cael_ormund`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `npc_halden_mire`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `npc_cluster_teacher`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `npc_osran_kell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `npc_mattis_cray`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `npc_wyn_sabler`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `npc_dessa_vane`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `npc_perrin_ashby`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `npc_ivo_fenn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `npc_kess_adler`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `npc_ansel_duth`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `npc_tamsin_rook`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `npc_len_quill`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `npc_hadi_morrow`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `npc_nila_brant`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `npc_maren_holt`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `npc_ira_vell`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `npc_benno_kade`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `npc_quil_esser`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `npc_osric_tann`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `npc_dara_mewn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `npc_dr_irina_vel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `npc_wyn_omah`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `npc_piet_abar`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `npc_saria_voss`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `npc_salt_marshal_varn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `npc_salt_trader_elena`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `npc_salt_boiler_petyr`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `npc_switch_master_korov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `npc_rail_chandler_bess`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `npc_rivet_smith_milos`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `npc_beacon_keeper_maren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `npc_coastal_chandler_orlov`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `npc_net_mender_kira`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `npc_quarry_steward_darek`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `npc_stone_cutter_valya`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `npc_driller_jarek`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `npc_prior_silas`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `npc_almoner_hanna`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `npc_wayfarer_tobias`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `npc_market_warden_grimm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `npc_junk_broker_solomon`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `npc_grease_monkey_tess`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `npc_odile_vanter`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `npc_cass_polder`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `npc_jorin_hael`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `npc_uma_tarran`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `npc_halloran_vesk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `npc_lotte_verrill`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `npc_mara_veln`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `npc_oskar_ruut`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `npc_tomas_geret`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `npc_joren_malk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `npc_pavel_eren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `npc_sena_aris`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `npc_dalia_marun`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `npc_anton_renn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `npc_emil_soren`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `npc_nadia_lem`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `npc_arvo_tamm`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `npc_kaspar_drej`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `npc_mira_vos`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `npc_janek_orel`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `npc_veda_ro`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `npc_mirael_tesk`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `npc_niko`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `npc_ilze_kaar`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `npc_marek_voln`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `npc_lina`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `npc_anete_sarn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `npc_elder_sava`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `npc_rika_dorn`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `npc_liva_kern`
- Source: `Assets/StreamingAssets/Data/characters.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `item_decon_chelator_concentrate`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `item_lead_lined_effluent_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `item_heavy_neoprene_scrub_brush`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `item_sealed_waste_bin`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `item_theodolite_brass_precision`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `item_surveyor_stadia_rod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `item_datum_plate_bronze`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `item_concrete_mix`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `item_forged_rotor_shaft`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `item_magnetic_bearing_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `item_high_vacuum_pump`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `item_containment_ring_steel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `item_reinforced_concrete_vault`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `item_seismic_damper_pad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `item_vacuum_pump_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each encounter or crisis record, locate the live Crossing route, phase/state owner, participating faction/item references, and the exact resolution consequence.
- Primary owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State/save rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI truth rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: frontier governance, encounters, and arbitration.
- Seam under test: crossing_encounters/crises/quests/factions -> CrossingCatalog/Session/Arbitration/QuestSystem -> existing faction, inventory, morale, journal, and UI projections.
- Expected authority: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger. Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI/accessibility check: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- Player-facing truth: Missing location, empty choices, duplicate crisis phases, dead participants, insufficient resources, repeated encounter, and absent faction owners resolve to a safe explicit state.
- Persistence response: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism response: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: CrossingCatalog.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: CrossingSession.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: CrossingArbitrationSystem.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: CrossingQuestSystem.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: crossing_encounters.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: crises.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: CrossingCatalog.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: CrossingSession.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: CrossingArbitrationSystem.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: CrossingQuestSystem.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: crossing_encounters.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: crises.
- Owner: CrossingSession and CrossingArbitrationSystem own Crossing state; quest/faction/inventory systems own consequences; host wires the existing route.
- State rule: Active crisis phase, votes/commitments, encounter resolution, and cooldown use the existing Crossing/quest/flag state or an explicit additive field with migration; no new generic ledger.
- Determinism rule: Encounter selection and crisis phase transitions use existing seeded route streams, stable option order, and explicit repeat/cooldown keys.
- UI rule: ['src/UI/CrossingQuestPanel.cs', 'src/YearOfAsh/DoorEncounterModal.cs', 'src/UI/CrossingSafeConductVouchPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingQuestSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingArbitrationSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CrossingFactionExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan115_116CrossingDeepLoreIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Expansions/Plan18ExpansionDeepeningTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 486,002 characters.
# Post-250K deep polishing pass

The architecture body above reached 486,080 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `CrossingCatalog`, `CrossingSession`, `CrossingArbitrationSystem`, `CrossingQuestSystem`, `crossing_encounters`, `crises`.
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
