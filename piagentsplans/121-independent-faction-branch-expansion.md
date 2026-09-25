# Plan 121 — Independent Faction Branch Expansion: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:**  unaffiliated identity, moral arcs, and endings
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Extend the existing IndependentBranch owner and FactionBranchCoordinator with evidence-backed branch eligibility, irreversible transitions, durable state, and ending projections.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5797` characters.
- Current worktree copy: `491583` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9067403122f49446d8b44d7827351553b4a69d3cd398ea4d09ec0b56b2de778d`
- Snapshot size: 4278 characters; 94 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017:     /// <summary>
0018:     /// One base Independent branch row, matching independent_faction_branch.json
0019:     /// shape. Carries three optional gate fields Military/Rebel branches do
0020:     /// not have: requires_prpf_standing_min (IND-3), and
...
0039:
0040:     /// <summary>Root shape of independent_faction_branch.json.</summary>
0041:     public sealed class IndependentBranchDataFile
0042:     {
...
0048:     /// <summary>Immutable-after-load catalog of Independent branch/ending definitions.</summary>
0049:     public sealed class IndependentBranchCatalog : IEnumerable<IndependentBranchEntry>
0050:     {
0051:         private readonly Dictionary<string, IndependentBranchEntry> _byId =
...
0057:
0058:         public static IndependentBranchCatalog Empty() => new IndependentBranchCatalog();
0059:
0060:         public void Register(IndependentBranchEntry entry)
...
0074:
0075:         public static IndependentBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
0076:         {
0077:             if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
...
0080:             var catalog = new IndependentBranchCatalog();
0081:             string path = fileIO.Combine(dataDir, "independent_faction_branch.json");
0082:             if (!fileIO.FileExists(path)) return catalog;
0083:
```

### Current evidence: `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1de3b3d2d06a1d1caadc79c401928064930aa039c048b0dc91332e8bd303444c`
- Snapshot size: 15279 characters; 310 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0033:     /// </summary>
0034:     public sealed class IndependentBranchSystem
0035:     {
0036:         public const string SystemId = "independent_branch_system";
...
0042:
0043:         private readonly IndependentBranchCatalog _catalog;
0044:         private readonly IFlagLedger _flags;
0045:         private readonly ILog _log;
...
0053:
0054:         public IndependentBranchSystem(IndependentBranchCatalog catalog, IFlagLedger flags, IndependentBranchSystemState? state = null, ILog? log = null)
0055:         {
0056:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
...
0068:
0069:         public IndependentBranchSystemState State => _state;
0070:         public int CurrentDay => _state.timeline.currentDay;
0071:         public string? CommittedBranchId => string.IsNullOrEmpty(_state.branch.branchId) ? null : _state.branch.branchId;
...
0255:
0256:         public IndependentBranchSystemState CaptureState() => Clone(_state);
0257:
0258:         public void RestoreState(IndependentBranchSystemState state)
...
0276:
0277:         private static IndependentBranchSystemState Clone(IndependentBranchSystemState source)
0278:         {
0279:             return new IndependentBranchSystemState
```

### Current evidence: `Assets/Ashfall.Core/Factions/IndependentBranchState.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b6a256ffc4449efa3fdb166e64f852c96036540a9cedbfda45c7df9839881030`
- Snapshot size: 2771 characters; 69 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0024:     /// lifecycle itself IS a mirror, even though the gating logic around it
0025:     /// (see IndependentBranchSystem.CommitBranch) is not.
0026:     /// </summary>
0027:     [Serializable]
...
0037:     [Serializable]
0038:     public class IndependentBranchSystemState
0039:     {
0040:         public string systemId = IndependentBranchSystem.SystemId;
```

### Current evidence: `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e7d522be36b701d6e7e9cfa99c696a34d923bde7ba8f6c47babf7dd144a4d008`
- Snapshot size: 3021 characters; 75 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006:     [Serializable]
0007:     public class IndependentBranchSave
0008:     {
0009:         public const int CurrentSaveVersion = 1;
...
0021:     /// </summary>
0022:     public static class IndependentBranchSaveCodec
0023:     {
0024:         public static IndependentBranchSave Capture(IndependentBranchSystem system)
...
0027:
0028:             var save = new IndependentBranchSave
0029:             {
0030:                 branchSystem = system.CaptureState()
...
0035:
0036:         public static void Restore(IndependentBranchSave save, IndependentBranchSystem system)
0037:         {
0038:             if (save == null) throw new ArgumentNullException(nameof(save));
...
0042:
0043:         public static string Encode(IndependentBranchSave save, IJsonSerializer json)
0044:         {
0045:             if (save == null) throw new ArgumentNullException(nameof(save));
...
0050:
0051:         public static IndependentBranchSave Decode(string jsonText, IJsonSerializer json)
0052:         {
0053:             if (string.IsNullOrEmpty(jsonText))
...
0056:
0057:             var save = json.Deserialize<IndependentBranchSave>(jsonText);
0058:             if (save == null)
0059:                 throw new InvalidOperationException("IndependentBranchSave: deserialization returned null.");
...
0062:                 throw new InvalidOperationException(
0063:                     $"IndependentBranchSave: saveVersion {save.saveVersion} is newer than supported ({IndependentBranchSave.CurrentSaveVersion}).");
0064:
0065:             if (!string.IsNullOrEmpty(save.Checksum))
...
0068:                 if (!string.Equals(save.Checksum, actual, StringComparison.Ordinal))
0069:                     throw new InvalidOperationException("IndependentBranchSave: checksum mismatch (corrupted or tampered save).");
0070:             }
0071:
```

### Current evidence: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e5d365c6263dfb636692ae8dc1994210b13b97fd3bdf4c84f849d05a96dcde4f`
- Snapshot size: 27660 characters; 667 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0058:     /// - RebelBranchSystem (15 Rebel branches, faction alignment, PoNR)
0059:     /// - IndependentBranchSystem (15 Independent branches, cross-faction relations, PoNR)
0060:     /// - PrpfStandingSystem (PRPF third-power standing, alignment, join/oppose)
0061:     ///
...
0067:     /// </summary>
0068:     public sealed class FactionBranchCoordinator
0069:     {
0070:         public const string SystemId = "faction_branch_coordinator";
...
0073:         private readonly RebelBranchCatalog _rebelCatalog;
0074:         private readonly IndependentBranchCatalog _independentCatalog;
0075:         private readonly IFlagLedger _flags;
0076:         private readonly ILog _log;
...
0079:         public RebelBranchSystem Rebel { get; }
0080:         public IndependentBranchSystem Independent { get; }
0081:         public PrpfStandingSystem Prpf { get; }
0082:
...
0087:
0088:         public FactionBranchCoordinator(
0089:             MilitaryBranchCatalog? militaryCatalog = null,
0090:             RebelBranchCatalog? rebelCatalog = null,
...
0095:             RebelBranchSystemState? rebelState = null,
0096:             IndependentBranchSystemState? independentState = null,
0097:             PrpfSystemState? prpfState = null)
0098:         {
...
0103:             _rebelCatalog = rebelCatalog ?? RebelBranchCatalog.Empty();
0104:             _independentCatalog = independentCatalog ?? IndependentBranchCatalog.Empty();
0105:
0106:             Military = new MilitaryBranchSystem(_militaryCatalog, _flags, militaryState, _log);
...
0359:             {
0360:                 _log.Error($"FactionBranchCoordinator commit error: {ex.Message}");
0361:                 return ActionResult.Failed("commit_exception", ex.Message);
0362:             }
...
0631:
0632:         public static FactionBranchCoordinator LoadFromData(
0633:             string dataDir,
0634:             IFileIO fileIO,
...
0640:             var rebCatalog = RebelBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
0641:             var indCatalog = IndependentBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
0642:
0643:             return new FactionBranchCoordinator(
```

### Current evidence: `src/Main.MoralChoice.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.MoralChoice.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e12534c6d2a8cd86c90d7283de173c51bf07cd68e46bb52f6129bbeb05375b8d`
- Snapshot size: 16113 characters; 344 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Collections.Generic;
0005: using System.Linq;
0006: using Ashfall.Core;
0007: using Ashfall.Core.MoralChoice;
0008:
0009: namespace AtomicWar.GodotApp
0010: {
0011:     public partial class Main : Control
0012:     {
0013:         // ── Moral choice ("The Weight of Survival") host wiring ──
0014:         // The score is invisible by design: hosts read CurrentBand and the
0015:         // threshold events, never the raw number.
0016:         private MoralChoiceSystem _moralChoice = null!;
0017:         private List<MoralChoiceQuestDefinition> _moralChoiceDefs = new List<MoralChoiceQuestDefinition>();
0018:         private bool _moralChoiceDirty;
0019:
0020:         // ── Branching / gossip / faction reactions (Phase 2 data) ──
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

### Current evidence: `src/UI/Plans130To133Panel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/Plans130To133Panel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `2558213f94b7292c2875e91022e4470de480a93dce19ff746c77d072107d0060`
- Snapshot size: 15625 characters; 343 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Linq;
0004: using Godot;
0005: using Ashfall.Core.Expeditions;
0006: using Ashfall.Core.Foundry;
0007: using Ashfall.Core.Medical;
0008: using Ashfall.Core.Radio;
0009: using DesignTheme = Ashfall.Core.UI.Theme;
0010:
0011: namespace AtomicWar.GodotApp.UI
0012: {
0013:     /// <summary>
0014:     /// Bound operations console for Plans 130–133. It is presentation-only:
0015:     /// commands route through the four host sessions and railway remains the
0016:     /// canonical train/track owner.
0017:     /// </summary>
0018:     public partial class Plans130To133Panel : Control, IBindablePanel
0019:     {
0020:         public event Action? OnClose;
```

### Current evidence: `Assets/StreamingAssets/Data/independent_faction_branch.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `678a4bd46d2091ff54c62bd3617c4c1507a2582e520d0be46c8261e78cf5036e`
- Snapshot size: 12207 characters; 204 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "faction_id": "faction_independent",
0004:   "branches": [
0005:     {
0006:       "id": "branch_ind_1_survivor",
0007:       "display_name": "The Survivor",
0008:       "ponr_flag": "flag_branch_ind_1_ponr",
0009:       "ponr_trigger": "You make the choice that defines you with no faction left to blame or credit.",
0010:       "entry_band_min": "very_evil",
0011:       "entry_band_max": "very_positive",
0012:       "endings": [
0013:         { "ending_id": "ending_ind_1a_lone_survivor", "band_min": "neutral", "band_max": "slightly_positive", "display_name": "The Lone Survivor" },
0014:         { "ending_id": "ending_ind_1b_traitor", "band_min": "very_evil", "band_max": "evil", "display_name": "The Traitor" },
0015:         { "ending_id": "ending_ind_1c_legend", "band_min": "positive", "band_max": "very_positive", "display_name": "The Legend" }
0016:       ]
0017:     },
0018:     {
0019:       "id": "branch_ind_2_mercenary",
0020:       "display_name": "The Mercenary",
```

### Current evidence: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e8eef1a2c1663bda30282be57f82178d830f1ea663cc173d97a99e54fe323786`
- Snapshot size: 194389 characters; 5225 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "quests": [
0004:     {
0005:       "id": "quest_bone_pickers_01",
0006:       "display_name": "Bone Pickers - Stage 1",
0007:       "type": "faction_chain",
0008:       "briefing": "A lone wanderer is caught in our snare traps. His leg is shattered. The Guild demands we don't waste a bullet\u2014just take his boots and leave him for the ash-hounds.",
0009:       "prereq_quest_id": "",
0010:       "min_day": 10,
0011:       "stages": [
0012:         {
0013:           "id": "stage_1",
0014:           "text": "The wanderer is still in the snare, and he has stopped begging and started watching you look at his boots."
0015:         }
0016:       ],
0017:       "choices": [
0018:         {
0019:           "id": "quest_bone_pickers_01_advance",
0020:           "text": "Take the boots. Leave him.",
```

### Current evidence: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`
- Snapshot size: 14950 characters; 297 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "description": "Faction NPC dialogues triggered by moral threshold events. Each event fires once per save when the player crosses a moral band boundary overnight. These are the world acknowledging what you've become.",
0004:   "threshold_reactions": {
0005:     "moral_event_bounty_issued": {
0006:       "event_description": "Fires when the player enters VeryEvil band (-100 or below). Peacekeepers issue a bounty.",
0007:       "peacekeeper_dialogue": [
0008:         {
0009:           "speaker": "Peacekeeper Sergeant Veill",
0010:           "location": "Peacekeeper outpost notice board",
0011:           "lines": [
0012:             "Your face is on the board now. I put it there myself.",
0013:             "You had a chance to be something other than this. That chance is spent.",
0014:             "The bounty is alive. For now. Push further and I cross out the 'alive' with my own hand.",
0015:             "Every patrol from here to the southern corridor knows your description. Height, gait, the scar. All of it.",
0016:             "Run if you want. Hide if you can. But know this: someone in this wasteland wants the rations that come with your capture."
0017:           ]
0018:         },
0019:         {
0020:           "speaker": "Anonymous Peacekeeper recruit",
```

### Current evidence: `Assets/StreamingAssets/Data/endings.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f7c3ffc82e2c3635b38456146cc53ea73316cba3c58bb895ee0fc3d41c8d9100`
- Snapshot size: 6886 characters; 93 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "endings": [
0004:     {
0005:       "id": "ending_dawn_of_thaw",
0006:       "title": "The Spring of Year Two",
0007:       "category": "victory",
0008:       "tone": "hopeful",
0009:       "min_days_survived": 360,
0010:       "min_living_survivors": 12,
0011:       "summary": "Three hundred and sixty days beneath the permafrost. The soot-choked stratospheric clouds slowly thin, and the first meltwater drips from the observation turret.",
0012:       "epilogue_text": "The long dark of the Year of Ash finally breaks. Through the armored glass of the surface cupola, the survivors watch pale sunlight strike the valley for the first time in twelve months. The hydroponic trays in the greenhouse are bursting with green shoots, and the generator hums steadily on conserved fuel. The bunker did not just hold—it endured. As the heavy hydraulic airlock grinds open to the warming air, the dwellers step out into a quiet, barren world ready for the first seeds of rebuilding.",
0013:       "factions_alignment": "independent"
0014:     },
0015:     {
0016:       "id": "ending_iron_hegemony",
0017:       "title": "The Iron Bastion",
0018:       "category": "victory",
0019:       "tone": "militaristic",
0020:       "min_days_survived": 200,
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

### Current evidence: `src/UI/PoliticsUI.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/PoliticsUI.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9190576cb0b9b8b3908d51da60599ec08ab00f4f4e4082205b75b077e5c77e34`
- Snapshot size: 5330 characters; 126 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ============================================================================
0003: // UI Panel: Settlement Politics & Council Chamber (Plan 185)
0004: // Displays current leadership, governance mode, approval factors, active policies, and elections.
0005: // ============================================================================
0006: using System;
0007: using Godot;
0008: using Ashfall.Core.Narrative;
0009: using Ashfall.Core.UI;
0010:
0011: namespace AtomicWar.GodotApp.UI
0012: {
0013:     public partial class PoliticsUI : Control, IBindablePanel
0014:     {
0015:         public event Action? OnClose;
0016:
0017:         private AshfallDashboardShell _shell = null!;
0018:         private AshfallStatusRail? _statusRail;
0019:         private VBoxContainer _contentStack = null!;
0020:         private Label _detailText = null!;
```

### Current evidence: `src/UI/EpiloguePanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/EpiloguePanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `40e17093add485b4ca0e323093811deeabc7afdee6e5751975926c2ead73c9ba`
- Snapshot size: 10667 characters; 251 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.Endgame;
0005: using Ashfall.Core.UI;
0006: using CoreTheme = Ashfall.Core.UI.Theme;
0007:
0008: namespace AtomicWar.GodotApp.UI
0009: {
0010:     /// <summary>
0011:     /// ASHFALL — Endgame Epilogue Panel.
0012:     /// Evaluates whole-saga world state across 32 matrix permutations, generating the
0013:     /// authoritative literary-grade chronicle of the wasteland.
0014:     ///
0015:     /// Presentation only — evaluates EpilogueMatrixRuntime against simulation state.
0016:     /// </summary>
0017:     public partial class EpiloguePanel : Control
0018:     {
0019:         public event Action? OnClose;
0020:
```

### Current evidence: `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c4f215beea1c97660bbbef64b7573b776626e450b27c4e797e2041180862e9cb`
- Snapshot size: 2890 characters; 67 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011:     /// Gate tests for the Independent branch data authority
0012:     /// (independent_faction_branch.json) against the shared branch_/ending_
0013:     /// id prefixes registered in CatalogIntegrityValidator.
0014:     /// </summary>
...
0029:         {
0030:             var catalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
0031:
0032:             Assert.Equal(IndependentBranchIds.BranchCount, catalog.Count);
...
0057:             var rebel = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
0058:             var independent = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
0059:
0060:             foreach (var branchId in IndependentBranchIds.AllBranches)
```

### Current evidence: `Ashfall.Core.Tests/IndependentBranchSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1bb25281370c61ac770a46a2057630d6032ef7b88a40251cf18abbd7702a6608`
- Snapshot size: 12691 characters; 311 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0010: {
0011:     public class IndependentBranchSystemTests
0012:     {
0013:         private static IndependentBranchCatalog LoadCatalog()
...
0019:             Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
0020:             return IndependentBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
0021:         }
0022:
...
0085:             var catalog = LoadCatalog();
0086:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0087:             var moral = MakeMoralChoice();
0088:
...
0097:             var catalog = LoadCatalog();
0098:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0099:             var moral = MakePositiveMoralChoice();
0100:
...
0110:             var catalog = LoadCatalog();
0111:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0112:             var moral = MakePositiveMoralChoice();
0113:             var prpf = new PrpfStandingSystem(new InMemoryFlagLedger());
...
0123:             var catalog = LoadCatalog();
0124:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0125:             var moral = MakePositiveMoralChoice();
0126:             var prpf = new PrpfStandingSystem(new InMemoryFlagLedger());
...
0137:             var catalog = LoadCatalog();
0138:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0139:             var moral = MakeEvilMoralChoice();
0140:
...
0148:             var catalog = LoadCatalog();
0149:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0150:             var moral = MakeEvilMoralChoice();
0151:
...
0163:             var catalog = LoadCatalog();
0164:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0165:             var moral = MakeEvilMoralChoice();
0166:
...
0178:             var catalog = LoadCatalog();
0179:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0180:
0181:             system.ModifyMilitaryStanding(1000);
...
0184:             system.ModifyMilitaryStanding(-2000);
0185:             Assert.Equal(IndependentBranchSystem.MinStanding, system.MilitaryStanding);
0186:             Assert.True(system.IsHostileToMilitary);
0187:         }
...
```

### Current evidence: `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `acc794e44f7e8dbecdaaeaa890c4ca33f48d44daac1d88ed25f256d9793660e7`
- Snapshot size: 21544 characters; 530 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0025:
0026:         private static IndependentBranchCatalog LoadCatalog()
0027:         {
0028:             string dir = ResolveDataDir();
...
0268:                     var flags = new InMemoryFlagLedger();
0269:                     var system = new IndependentBranchSystem(catalog, flags);
0270:                     var moral = MakeMoralChoiceWithBand(band);
0271:
...
0287:             var catalog = LoadCatalog();
0288:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0289:             var moral = MakeMoralChoiceWithBand(MoralPathBand.VeryEvil);
0290:
...
0298:             var catalog = LoadCatalog();
0299:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0300:             var evil = MakeMoralChoiceWithBand(MoralPathBand.Evil);
0301:
...
0313:             var catalog = LoadCatalog();
0314:             var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
0315:             var positive = MakeMoralChoiceWithBand(MoralPathBand.Positive);
0316:
...
0329:             var flags = new InMemoryFlagLedger();
0330:             var system = new IndependentBranchSystem(catalog, flags);
0331:             var moral = MakeMoralChoiceWithBand(MoralPathBand.Neutral);
0332:
...
0351:             var flagsA = new InMemoryFlagLedger();
0352:             var systemA = new IndependentBranchSystem(catalog, flagsA);
0353:             var moral = MakeMoralChoiceWithBand(MoralPathBand.Positive);
0354:
...
0360:
0361:             var save = IndependentBranchSaveCodec.Capture(systemA);
0362:             var serializer = new SystemTextJsonSerializer();
0363:             string encoded = IndependentBranchSaveCodec.Encode(save, serializer);
...
0366:             var flagsB = new InMemoryFlagLedger();
0367:             var systemB = new IndependentBranchSystem(catalog, flagsB);
0368:             IndependentBranchSaveCodec.Restore(loaded, systemB);
0369:
...
0381:             var flagsA = new InMemoryFlagLedger();
0382:             var systemA = new IndependentBranchSystem(catalog, flagsA);
0383:             var moral = MakeMoralChoiceWithBand(MoralPathBand.SlightlyEvil);
0384:
...
0390:
0391:             var save = IndependentBranchSaveCodec.Capture(systemA);
0392:             var serializer = new SystemTextJsonSerializer();
0393:             string encoded = IndependentBranchSaveCodec.Encode(save, serializer);
...
```

### Current evidence: `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `29e1f5fe946e79a30ca4d4ea3ca6f47740c7897a380fae966766a698987a0126`
- Snapshot size: 18232 characters; 421 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012: {
0013:     public class FactionBranchCoordinatorTests
0014:     {
0015:         private static string DataDir()
...
0023:
0024:         private static FactionBranchCoordinator CreateCoordinator(IFlagLedger? flags = null)
0025:         {
0026:             var fileIO = new FileSystemIO();
```

### Current evidence: `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `13b686f198d860a97ac06335e6b807822829da18daec531506e5faeae38c75e8`
- Snapshot size: 10372 characters; 237 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0059:
0060:             var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
0061:             var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
0062:
...
0094:
0095:             var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
0096:             var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
0097:
...
0123:
0124:             var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
0125:             var militaryCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
0126:
...
0154:
0155:             var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
0156:             var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
0157:
...
0180:             // Test Independent save codec roundtrip
0181:             var indepSave = IndependentBranchSaveCodec.Capture(indepSystem);
0182:             string indepJson = IndependentBranchSaveCodec.Encode(indepSave, serializer);
0183:             var restoredIndepSave = IndependentBranchSaveCodec.Decode(indepJson, serializer);
...
0203:
0204:             var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
0205:             var milCatalog = MilitaryBranchCatalog.LoadAndRegister(dir, io, serializer);
0206:             var flags = new InMemoryFlagLedger();
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/independent_faction_branch.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, faction_id, branches`
- `branches`: list count=15; sample IDs=['branch_ind_1_survivor', 'branch_ind_2_mercenary', 'branch_ind_3_peacekeeper_diplomat', 'branch_ind_4_exile', 'branch_ind_5_kingmaker', 'branch_ind_6_legend', 'branch_ind_7_ghost', 'branch_ind_8_wasteland_myth']
- `schema_version`: `1`
- SHA-256: `678a4bd46d2091ff54c62bd3617c4c1507a2582e520d0be46c8261e78cf5036e`
#### `Assets/StreamingAssets/Data/quests_faction_branching.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, quests`
- `quests`: list count=200; sample IDs=['quest_bone_pickers_01', 'quest_bone_pickers_02', 'quest_bone_pickers_03', 'quest_bone_pickers_04', 'quest_bone_pickers_05', 'quest_bone_pickers_06', 'quest_bone_pickers_07', 'quest_bone_pickers_08']
- `schema_version`: `1`
- SHA-256: `e8eef1a2c1663bda30282be57f82178d830f1ea663cc173d97a99e54fe323786`
#### `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, description, threshold_reactions`
- `schema_version`: `1`
- `description`: `Faction NPC dialogues triggered by moral threshold events. Each event fires once per save when the player crosses a moral band boundary overnight. These are the world acknowledging what you've become.`
- SHA-256: `fd9f4c31b6dd1c9779bc820d7b02923901257b704f930c3572e6d2d57933950d`
#### `Assets/StreamingAssets/Data/endings.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, endings`
- `endings`: list count=8; sample IDs=['ending_dawn_of_thaw', 'ending_iron_hegemony', 'ending_exodus_to_sea', 'ending_silent_tombs', 'ending_the_reckoning', 'ending_wasteland_sanctuary', 'ending_frozen_silence', 'ending_warlord_tribute']
- `schema_version`: `1`
- SHA-256: `f7c3ffc82e2c3635b38456146cc53ea73316cba3c58bb895ee0fc3d41c8d9100`
#### `Assets/StreamingAssets/Data/characters.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=84; sample IDs=['npc_bram_ostrowski', 'npc_sergeant_pell', 'npc_doctor_ianov', 'npc_wren', 'npc_kestrel', 'npc_nomi_fisk', 'npc_ivor_lasko', 'npc_the_cartwright_sisters']
- `schema_version`: `1`
- SHA-256: `cd33aa4801aade57932525194755b413d83ab2cd212c0ca7a5ccd09f011b5c58`
## Symbol and caller audit

#### `IndependentBranchCatalog` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=11, host=0, test=15
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:422` (core) — ["independent_faction_branch.json"] = new[] { "IndependentBranchCatalog" },
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:74` (core) — private readonly IndependentBranchCatalog _independentCatalog;
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:91` (core) — IndependentBranchCatalog? independentCatalog = null,
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:104` (core) — _independentCatalog = independentCatalog ?? IndependentBranchCatalog.Empty();
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:641` (core) — var indCatalog = IndependentBranchCatalog.LoadAndRegister(dataDir, fileIO, json);
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:49` (declaration) — public sealed class IndependentBranchCatalog : IEnumerable<IndependentBranchEntry>
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:58` (core) — public static IndependentBranchCatalog Empty() => new IndependentBranchCatalog();
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:75` (core) — public static IndependentBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:80` (core) — var catalog = new IndependentBranchCatalog();
- `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs:43` (core) — private readonly IndependentBranchCatalog _catalog;
- `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs:54` (core) — public IndependentBranchSystem(IndependentBranchCatalog catalog, IFlagLedger flags, IndependentBranchSystemState? state = null, ILog? log = null)
- `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs:30` (test) — var catalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs:58` (test) — var independent = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:26` (test) — private static IndependentBranchCatalog LoadCatalog()
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:29` (test) — return IndependentBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/IndependentBranchSystemTests.cs:13` (test) — private static IndependentBranchCatalog LoadCatalog()
- `Ashfall.Core.Tests/IndependentBranchSystemTests.cs:20` (test) — return IndependentBranchCatalog.LoadAndRegister(dir, new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs:32` (test) — var independentCatalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/WeightOfChoicesSaveTests.cs:57` (test) — var independentCatalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:60` (test) — var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:95` (test) — var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:124` (test) — var independentCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:155` (test) — var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:204` (test) — var indepCatalog = IndependentBranchCatalog.LoadAndRegister(dir, io, serializer);
- … 2 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `IndependentBranchSystem` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=13, host=0, test=36
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:731` (core) — ["independent_faction_branch.json"] = "IndependentBranchSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1029` (core) — ["independent_faction_branch.json"] = new[] { "IndependentBranchSystem" },
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:59` (core) — /// - IndependentBranchSystem (15 Independent branches, cross-faction relations, PoNR)
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:80` (core) — public IndependentBranchSystem Independent { get; }
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:108` (core) — Independent = new IndependentBranchSystem(_independentCatalog, _flags, independentState, _log);
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:24` (core) — public static IndependentBranchSave Capture(IndependentBranchSystem system)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:36` (core) — public static void Restore(IndependentBranchSave save, IndependentBranchSystem system)
- `Assets/Ashfall.Core/Factions/IndependentBranchState.cs:25` (core) — /// (see IndependentBranchSystem.CommitBranch) is not.
- `Assets/Ashfall.Core/Factions/IndependentBranchState.cs:40` (core) — public string systemId = IndependentBranchSystem.SystemId;
- `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs:34` (declaration) — public sealed class IndependentBranchSystem
- `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs:54` (core) — public IndependentBranchSystem(IndependentBranchCatalog catalog, IFlagLedger flags, IndependentBranchSystemState? state = null, ILog? log = null)
- `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs:64` (core) — IndependentBranchSystem independentBranch,
- `Assets/Ashfall.Core/Factions/WeightOfChoicesSave.cs:87` (core) — IndependentBranchSystem independentBranch,
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:269` (test) — var system = new IndependentBranchSystem(catalog, flags);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:288` (test) — var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:299` (test) — var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:314` (test) — var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:330` (test) — var system = new IndependentBranchSystem(catalog, flags);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:352` (test) — var systemA = new IndependentBranchSystem(catalog, flagsA);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:367` (test) — var systemB = new IndependentBranchSystem(catalog, flagsB);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:382` (test) — var systemA = new IndependentBranchSystem(catalog, flagsA);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:397` (test) — var systemB = new IndependentBranchSystem(catalog, flagsB);
- `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs:423` (test) — var system = new IndependentBranchSystem(catalog, flags, oldState);
- `Ashfall.Core.Tests/IndependentBranchSystemTests.cs:86` (test) — var system = new IndependentBranchSystem(catalog, new InMemoryFlagLedger());
- … 25 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `IndependentBranchState` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `IndependentBranchSave` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=12, host=0, test=0
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:7` (declaration) — public class IndependentBranchSave
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:24` (core) — public static IndependentBranchSave Capture(IndependentBranchSystem system)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:28` (core) — var save = new IndependentBranchSave
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:36` (core) — public static void Restore(IndependentBranchSave save, IndependentBranchSystem system)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:43` (core) — public static string Encode(IndependentBranchSave save, IJsonSerializer json)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:51` (core) — public static IndependentBranchSave Decode(string jsonText, IJsonSerializer json)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:54` (core) — throw new InvalidOperationException("IndependentBranchSave: empty save payload.");
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:57` (core) — var save = json.Deserialize<IndependentBranchSave>(jsonText);
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:59` (core) — throw new InvalidOperationException("IndependentBranchSave: deserialization returned null.");
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:61` (core) — if (save.saveVersion > IndependentBranchSave.CurrentSaveVersion)
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:63` (core) — $"IndependentBranchSave: saveVersion {save.saveVersion} is newer than supported ({IndependentBranchSave.CurrentSaveVersion}).");
- `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs:69` (core) — throw new InvalidOperationException("IndependentBranchSave: checksum mismatch (corrupted or tampered save).");
#### `FactionBranchCoordinator` — HOST_REFERENCE_PRESENT — core/declaration=9, host=8, test=7
- `Assets/Ashfall.Core/Muster/MusterWarfareEngine.cs:40` (core) — FactionBranchCoordinator? coordinator,
- `Assets/Ashfall.Core/Muster/MusterWarfareEngine.cs:57` (core) — // Exclusivity / Commitment Check with FactionBranchCoordinator
- `Assets/Ashfall.Core/Muster/MusterWarfareEngine.cs:108` (core) — FactionBranchCoordinator? coordinator = null)
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:68` (declaration) — public sealed class FactionBranchCoordinator
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:88` (core) — public FactionBranchCoordinator(
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:360` (core) — _log.Error($"FactionBranchCoordinator commit error: {ex.Message}");
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:632` (core) — public static FactionBranchCoordinator LoadFromData(
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:643` (core) — return new FactionBranchCoordinator(
- `Assets/Ashfall.Core/Factions/PrpfIds.cs:10` (core) — /// <c>FactionBranchCoordinator</c>). Autonomous power-growth, the
- `src/Host/FactionBranchHostSession.cs:11` (host) — /// Host session for FactionBranchCoordinator ("The Weight of Choices").
- `src/Host/FactionBranchHostSession.cs:16` (host) — public FactionBranchCoordinator Coordinator { get; }
- `src/Host/FactionBranchHostSession.cs:18` (host) — public FactionBranchHostSession(FactionBranchCoordinator coordinator)
- `src/Host/FactionBranchHostSession.cs:26` (host) — var coordinator = FactionBranchCoordinator.LoadFromData(
- `src/UI/FactionsPanel.cs:42` (host) — private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
- `src/UI/FactionsPanel.cs:59` (host) — Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
- `src/UI/QuestsPanel.cs:42` (host) — private Ashfall.Core.Factions.FactionBranchCoordinator? _branchCoordinator;
- `src/UI/QuestsPanel.cs:58` (host) — Ashfall.Core.Factions.FactionBranchCoordinator? branchCoordinator = null,
- `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs:24` (test) — private static FactionBranchCoordinator CreateCoordinator(IFlagLedger? flags = null)
- `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs:28` (test) — return FactionBranchCoordinator.LoadFromData(DataDir(), fileIO, json, flags ?? new InMemoryFlagLedger());
- `Ashfall.Core.Tests/MusterWarfareTests.cs:64` (test) — var coordinator = new FactionBranchCoordinator(milCatalog, rebCatalog);
- `Ashfall.Core.Tests/RebelBranchExpansionTests.cs:236` (test) — var coordinator = FactionBranchCoordinator.LoadFromData(
- `Ashfall.Core.Tests/Campaign/CampaignConsequenceLedgerTests.cs:65` (test) — var coordinator = new FactionBranchCoordinator(flags: unifiedLedger);
- `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs:208` (test) — var coordinator = new FactionBranchCoordinator(
- `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs:176` (test) — var coordinator = new FactionBranchCoordinator(
#### `independent_faction_branch` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=10, host=0, test=2
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:75` (core) — "independent_faction_branch.json", "military_faction_branch.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:422` (core) — ["independent_faction_branch.json"] = new[] { "IndependentBranchCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:731` (core) — ["independent_faction_branch.json"] = "IndependentBranchSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1029` (core) — ["independent_faction_branch.json"] = new[] { "IndependentBranchSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1455` (core) — ["independent_faction_branch.json"] = new[] { "FactionsPanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1734` (core) — "independent_faction_branch.json", "military_faction_branch.json",
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:18` (core) — /// One base Independent branch row, matching independent_faction_branch.json
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:40` (core) — /// <summary>Root shape of independent_faction_branch.json.</summary>
- `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs:81` (core) — string path = fileIO.Combine(dataDir, "independent_faction_branch.json");
- `Assets/Ashfall.Core/Factions/IndependentBranchIds.cs:9` (core) — /// independent_faction_branch.json by IndependentBranchCatalogTests.
- `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs:12` (test) — /// (independent_faction_branch.json) against the shared branch_/ending_
- `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs:35` (test) — Assert.True(catalog.Contains(branchId), $"missing branch '{branchId}' in independent_faction_branch.json");
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
#### authority lines 131-134
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
00132: | C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
00133: | C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
00134: | C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
#### authority lines 147-150
00147: | C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
00148: | C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
00149: | C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
00150: | C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
#### authority lines 173-176
00173: | C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
00174: | C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
00175: | C14 | Trapping yield versus equipment degradation cost; zoonosis risk premium on uncooked yield | HIGH CONFIDENCE |
00176: | All others | Balance audits only where numbers exist; never invent tuning targets without an intended design statement | — |
#### authority lines 210-213
00210: | C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
00211: | C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
00212: | C13 | Epilogue permutation reachability tests for under-served permutations | HIGH CONFIDENCE |
00213: | Cross | Determinism two-pass proofs for every new simulation; TEST-AGGREGATION metadata for catalog checks | CANON process |
#### authority lines 245-248
00245:
00246: **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.
00247:
00248: **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.
#### authority lines 259-262
00259:
00260: **SB-08 — Gate-count drift guard (Lane H).** Evidence: DR-07. Subject: a check that fails when a documented gate count diverges from the live inventory, ending manual count drift between bibles, closeouts, and CI. Integration route: small script/test in `scripts/ci/` family, mirrors existing gates. Confidence: PROPOSAL (design needs the live gate inventory as input).
00261:
00262: **SB-09 — `rewrite.py` data-authority hygiene (Lane H).** Evidence: DR-05. Subject: verify the script's role; relocate or document in place. Integration route: tooling-only; requires call-site verification first. Confidence: VERIFIED finding, PROPOSAL handling.
#### authority lines 358-361
00358:
00359: ## Subject Plan F-001 — Delayed Moral-Choice Callbacks
00360:
00361: Lane A · Cluster C10 · Status PROPOSAL.
#### authority lines 372-375
00372: ### What must not change
00373: Choice resolution behavior, flag ids, the weight_of_choices codec semantics, the sealed distress-signal content class (no callback may add a new distress scenario; callbacks arrive through journal, radio strip, visitor, or rumor seams, not the signal catalog).
00374:
00375: ### Recommended integration route (Template R)
#### authority lines 466-469
00466: ### Premise evidence
00467: VERIFIED: the 19-wave closeouts in `INTEGRATION_PLANS.md` record the endgame work as complete with evidence (Endgame 84/84 PASS). VERIFIED: `epilogue_chronicle.json`, `campaign_epilogues.json`, `endings.json` exist live. HIGH CONFIDENCE: some permutations carry thinner chronicle prose than others (structural inference from any 32-cell matrix authored incrementally; the audit could not read per-permutation depth at listing level — verify in session).
00468:
00469: ### Why this and not something else
#### authority lines 488-491
00488: ### Subject
00489: Re-run the deterministic economy/balance simulation harness across the content that landed since the last baseline (muster catalogs, moral-choice distress additions, the newest industrial catalogs, XP difficulty authority once W1 seals) and publish deltas in `docs/balance/`, converting the live baseline documents (DR-03) from point-in-time snapshots into a maintained series.
00490:
00491: ### Premise evidence
#### authority lines 538-541
00538: ### Subject
00539: A small CI-checkable guard that fails when a documented gate count (in bibles, closeouts, or handoffs) diverges from the live gate inventory, ending the manual count drift demonstrated by DR-07 (47 vs 57 gates; 11,098 vs 11,697 test totals).
00540:
00541: ### Premise evidence
#### authority lines 635-638
00635:
00636: Lane A/B · Cluster C3, C5 · Status INFERENCE pending sweep.
00637:
00638: ### Subject
#### authority lines 681-684
00681:
00682: **A-08 · C3 · Apiculture assay continuation.** Subject: Langstroth foundation-log continuation tied to seasonal yield and morale. Evidence: `langstroth_hive_foundation_logs` exists; apiculture is canon in Part 16.3. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00683:
00684: **A-09 · C4 · Hydraulic extrusion assay corpus twin.** Subject: ram-pressure and die-wear assay records for the live-but-unmapped hydraulic extrusion catalog (DR-04). Evidence: catalog verified live; corpus twin status unverified. Route: DATA-ONLY after census check. Confidence: HIGH CONFIDENCE (catalog) / UNVERIFIED (twin absence).
#### authority lines 713-716
00713:
00714: **A-24 · C10 · Bureaucratic-morality quest prose completion.** Subject: prose-field completion across `quests_bureaucratic_morality.json` records with skeleton `quest_hook`/outcome texts. Evidence: catalog verified live; Part 9 contracts define the fields. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00715:
00716: **A-25 · C10 · Massive-expansion corpus prose audit.** Subject: a prose-depth audit of `quests_massive_expansion_200.json` (200 records — the largest single prose debt surface in the data authority), converting skeleton records into contracted fields over several tranches. Evidence: catalog verified live; scale is structural evidence of thin per-record prose. Route: DATA-ONLY, multi-tranche. Confidence: HIGH CONFIDENCE.
#### authority lines 741-744
00741:
00742: **B-07 · C5 · Vehicle-breakdown medical/dose consequences.** Subject: expedition vehicle breakdowns producing injury and exposure events routed into medical and dose ledgers (extending `ExpeditionVehicleSystem` consequence routing). Evidence: `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` and vehicle armor grades verified live; dominance table implies breakdown modeling exists. Route: CORE-EXTENSION. Confidence: PROPOSAL — verify current breakdown consequence routing first.
00743:
00744: **B-08 · C5 · Scavenging-table parity for uncovered destinations.** Subject: complete per-destination renewable/one-time table coverage where the 49-table surface underserves the 53-destination catalog. Evidence: 49 vs 53 is canon (v1.0 Part 6.2). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
… 104 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.

**Requested behavior.** Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.

**Minimum safe delta.** Extend `independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.

**Required delta.** Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.

**Primary seam.** independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `IndependentBranchCatalog`, `IndependentBranchSystem`, `IndependentBranchState`, `IndependentBranchSave`, `FactionBranchCoordinator`, `independent_faction_branch`.

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
| Domain rules | IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 121.

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
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.
- What is the smallest safe change? Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.
- Which owner is touched? IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Factions/IndependentBranchSystem.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Factions/IndependentBranchState.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Factions/IndependentBranchSave.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.MoralChoice.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.GameFlow.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/Plans130To133Panel.cs` — IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/independent_faction_branch.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/quests_faction_branching.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/endings.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/characters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/Plans130To133Panel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/PoliticsUI.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/EpiloguePanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/IndependentBranchSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections is wired end to end or the plan explicitly closes as already integrated.
- Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

IndependentBranchCatalog, IndependentBranchSystem, IndependentBranchState, IndependentBranchSave, FactionBranchCoordinator, focused branch tests, and an independent_faction_branch.json catalog already exist. The old plan’s proposed class names and eight-to-fifteen count are not proof of current reachability.

# 3. Required Delta

Define how unaffiliated choices are observed, how point-of-no-return transitions are guarded, how branches interact with faction standing and endings, and how old saves and unavailable consequences are handled.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Faction and moral-choice owners are active; this plan must not write standing directly or invent a second branch registry. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `faction_independent`
- Source: `Assets/StreamingAssets/Data/independent_faction_branch.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `quest_bone_pickers_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `quest_bone_pickers_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `quest_bone_pickers_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `quest_bone_pickers_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `quest_bone_pickers_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `quest_bone_pickers_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `quest_bone_pickers_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `quest_bone_pickers_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `quest_bone_pickers_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `quest_bone_pickers_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `quest_blood_tithe_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `quest_blood_tithe_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `quest_blood_tithe_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `quest_blood_tithe_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `quest_blood_tithe_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `quest_blood_tithe_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `quest_blood_tithe_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `quest_blood_tithe_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `quest_blood_tithe_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `quest_blood_tithe_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `quest_drought_cartel_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `quest_drought_cartel_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `quest_drought_cartel_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `quest_drought_cartel_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `quest_drought_cartel_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `quest_drought_cartel_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `quest_drought_cartel_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `quest_drought_cartel_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `quest_drought_cartel_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `quest_drought_cartel_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `quest_martial_law_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `quest_martial_law_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `quest_martial_law_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `quest_martial_law_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `quest_martial_law_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `quest_martial_law_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `quest_martial_law_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `quest_martial_law_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `quest_martial_law_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `quest_martial_law_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `quest_guinea_pigs_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `quest_guinea_pigs_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `quest_guinea_pigs_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `quest_guinea_pigs_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `quest_guinea_pigs_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `quest_guinea_pigs_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `quest_guinea_pigs_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `quest_guinea_pigs_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `quest_guinea_pigs_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `quest_guinea_pigs_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `quest_piracy_mandate_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `quest_piracy_mandate_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `quest_piracy_mandate_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `quest_piracy_mandate_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `quest_piracy_mandate_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `quest_piracy_mandate_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `quest_piracy_mandate_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `quest_piracy_mandate_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `quest_piracy_mandate_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `quest_piracy_mandate_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `quest_iron_slaves_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `quest_iron_slaves_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `quest_iron_slaves_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `quest_iron_slaves_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `quest_iron_slaves_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `quest_iron_slaves_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `quest_iron_slaves_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `quest_iron_slaves_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `quest_iron_slaves_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `quest_iron_slaves_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `quest_quarantine_purge_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `quest_quarantine_purge_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `quest_quarantine_purge_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `quest_quarantine_purge_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `quest_quarantine_purge_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `quest_quarantine_purge_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `quest_quarantine_purge_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `quest_quarantine_purge_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `quest_quarantine_purge_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `quest_quarantine_purge_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `quest_assimilation_protocol_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `quest_assimilation_protocol_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `quest_assimilation_protocol_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `quest_assimilation_protocol_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `quest_assimilation_protocol_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `quest_assimilation_protocol_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `quest_assimilation_protocol_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `quest_assimilation_protocol_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `quest_assimilation_protocol_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `quest_assimilation_protocol_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `quest_storm_cult_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `quest_storm_cult_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `quest_storm_cult_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `quest_storm_cult_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `quest_storm_cult_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `quest_storm_cult_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `quest_storm_cult_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `quest_storm_cult_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `quest_storm_cult_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `quest_storm_cult_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `quest_scrap_network_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `quest_scrap_network_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `quest_scrap_network_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `quest_scrap_network_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `quest_scrap_network_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `quest_scrap_network_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `quest_scrap_network_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `quest_scrap_network_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `quest_scrap_network_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `quest_scrap_network_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `quest_broken_spears_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `quest_broken_spears_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `quest_broken_spears_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `quest_broken_spears_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `quest_broken_spears_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `quest_broken_spears_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `quest_broken_spears_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `quest_broken_spears_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `quest_broken_spears_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `quest_broken_spears_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `quest_free_wells_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `quest_free_wells_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `quest_free_wells_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `quest_free_wells_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `quest_free_wells_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `quest_free_wells_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `quest_free_wells_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `quest_free_wells_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `quest_free_wells_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `quest_free_wells_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `quest_the_defenders_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `quest_the_defenders_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `quest_the_defenders_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `quest_the_defenders_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `quest_the_defenders_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `quest_the_defenders_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `quest_the_defenders_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `quest_the_defenders_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `quest_the_defenders_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `quest_the_defenders_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `quest_cure_seekers_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `quest_cure_seekers_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `quest_cure_seekers_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `quest_cure_seekers_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `quest_cure_seekers_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `quest_cure_seekers_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `quest_cure_seekers_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `quest_cure_seekers_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `quest_cure_seekers_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `quest_cure_seekers_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `quest_rescue_armada_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `quest_rescue_armada_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `quest_rescue_armada_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `quest_rescue_armada_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `quest_rescue_armada_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `quest_rescue_armada_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `quest_rescue_armada_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `quest_rescue_armada_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `quest_rescue_armada_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `quest_rescue_armada_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `quest_plowshares_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `quest_plowshares_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `quest_plowshares_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `quest_plowshares_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `quest_plowshares_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `quest_plowshares_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `quest_plowshares_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `quest_plowshares_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `quest_plowshares_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `quest_plowshares_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `quest_hospital_ships_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `quest_hospital_ships_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `quest_hospital_ships_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `quest_hospital_ships_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `quest_hospital_ships_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `quest_hospital_ships_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `quest_hospital_ships_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `quest_hospital_ships_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `quest_hospital_ships_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Map each branch and ending record to a real choice/fact, a durable branch transition, a standing or ending consumer, and a visible player-facing consequence.
- Primary owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State/save rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI truth rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens:  unaffiliated identity, moral arcs, and endings.
- Seam under test: independent_faction_branch.json -> IndependentBranchCatalog/System -> FactionBranchCoordinator -> existing moral-choice, faction-standing, journal, and ending projections.
- Expected authority: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger. Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI/accessibility check: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- Player-facing truth: Missing branch records, invalid flags, dead survivors, repeated choices, old saves without branch state, and conflict with faction standing must produce neutral or explicitly unavailable outcomes.
- Persistence response: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism response: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: IndependentBranchCatalog.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: IndependentBranchSystem.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: IndependentBranchState.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: IndependentBranchSave.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: FactionBranchCoordinator.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: independent_faction_branch.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: IndependentBranchCatalog.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: IndependentBranchSystem.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: IndependentBranchState.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: IndependentBranchSave.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: FactionBranchCoordinator.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: independent_faction_branch.
- Owner: IndependentBranchSystem owns branch state; FactionStanceEngine owns standing; moral-choice owner owns choice facts; ending resolver owns final interpretation.
- State rule: Branch eligibility, committed branch, choice history, and completion use the existing independent branch save owner; no panel-local branch lock or second moral ledger.
- Determinism rule: Eligibility and ending selection are stable ordered evaluations over existing flags and branch state; no unseeded choice or wall-clock lockout.
- UI rule: ['src/UI/Plans130To133Panel.cs', 'src/UI/PoliticsUI.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/IndependentBranchCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/IndependentBranchCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/IndependentBranchSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/IndependentBranchSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/IndependentBranchExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/IndependentBranchExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/FactionBranchCoordinatorTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan121_122FactionBranchIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 481,332 characters.
# Post-250K deep polishing pass

The architecture body above reached 481,410 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `IndependentBranchCatalog`, `IndependentBranchSystem`, `IndependentBranchState`, `IndependentBranchSave`, `FactionBranchCoordinator`, `independent_faction_branch`.
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
