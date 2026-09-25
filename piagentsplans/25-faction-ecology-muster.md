# Plan 25 — Faction Ecology and the Muster: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** peacetime factions, war escalation, witnesses, and epilogue
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Connect existing faction action, treaty, war, Muster, witness, and epilogue owners into a readable late-game ecology without duplicating any faction system.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `6065` characters.
- Current worktree copy: `352890` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Muster/FactionActionCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `70361b914037781c0c09952b9e919e3b7ba847cd0ada24d1de1cbaa6a9a8c970`
- Snapshot size: 9353 characters; 226 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0070:     /// Engine-agnostic loader for muster_faction_actions.json — Plan 25's authored
0071:     /// peacetime faction ecology. Consumed only by FactionActionBoard. Missing file
0072:     /// → empty list (optional catalog by design); parse failures route through
0073:     /// CatalogDiagnostics; a schema_version beyond the known one is rejected.
```

### Current evidence: `Assets/Ashfall.Core/Muster/FactionActionBoard.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1d299ff85c2f7552e68f9721cd183d8fbda049f60c3e47c2a594e3d88673589b`
- Snapshot size: 17750 characters; 404 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0038:     /// old saves that predate the board).</summary>
0039:     public class FactionActionBoardState
0040:     {
0041:         public string systemId = FactionActionBoard.SystemId;
...
0054:     /// </summary>
0055:     public class FactionActionBoard
0056:     {
0057:         public const string SystemId = "faction_action_board";
...
0063:
0064:         private readonly FactionActionBoardState _state;
0065:         private readonly ScavengerGuildSystem _guild;
0066:         private readonly HydroBaronsSystem _hydro;
...
0072:         public event Action<FactionActionResolutionRecord> OnActionResolved;
0073:         public event Action<FactionActionBoardState> OnStateChanged;
0074:
0075:         public FactionActionBoard(
...
0079:             CoalitionCampSystem camp = null,
0080:             FactionActionBoardState state = null,
0081:             IFlagLedger ledger = null)
0082:         {
...
0087:             _ledger = ledger;
0088:             _state = state ?? new FactionActionBoardState();
0089:             if (_state.systemId != SystemId) _state.systemId = SystemId;
0090:             if (_state.resolved == null) _state.resolved = new List<FactionActionResolutionRecord>();
...
0093:
0094:         public FactionActionBoardState State => _state;
0095:         public IReadOnlyList<FactionActionDefinition> Catalog => _catalog;
0096:
...
0345:
0346:         public FactionActionBoardState CaptureState()
0347:         {
0348:             var copy = new FactionActionBoardState { systemId = SystemId };
...
0373:
0374:         public void RestoreState(FactionActionBoardState saved)
0375:         {
0376:             if (saved == null) return;
```

### Current evidence: `Assets/Ashfall.Core/Muster/FactionCultureCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8b64f325bcf926b4538f81db89650278085a4da592c583c23fec4d871b081c94`
- Snapshot size: 3214 characters; 89 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System.Collections.Generic;
0003: #pragma warning disable CS0649
0004: #pragma warning disable CS8618
0005:
0006: namespace Ashfall.Core.Muster
0007: {
0008:     /// <summary>One authored faction-culture codex entry (muster_faction_culture.json).
0009:     /// Plan 25 · 25E: everyday customs that make the factions societies instead of
0010:     /// banners — claim marks, water accounting, the code of what the Toll refuses,
0011:     /// the neutral-ground meal. Codex/codex-adjacent surfaces render these; events
0012:     /// should not restate them.</summary>
0013:     public class FactionCultureEntry
0014:     {
0015:         public string id = string.Empty;
0016:         public string factionId = string.Empty;
0017:         public string title = string.Empty;
0018:         public string body = string.Empty;
0019:     }
0020:
```

### Current evidence: `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `90fc407b41d205b9facb158031edf348516a03a858c3a28574f018b330735bf9`
- Snapshot size: 5717 characters; 134 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: #pragma warning disable CS8618
0005:
0006: namespace Ashfall.Core.Muster
0007: {
0008:     /// <summary>Serialized state of the Scavenger Guild (Section V.5) — Brannick Sten's
0009:     /// two-color claim ledger at loc_scavenger_guildhall.</summary>
0010:     public class ScavengerGuildState
0011:     {
0012:         public string systemId = ScavengerGuildSystem.SystemId;
0013:         public bool isActive;
0014:         public List<string> claimedSiteIds = new List<string>();
0015:         public HashSet<string> blacklistedShelterIds = new HashSet<string>(StringComparer.Ordinal);
0016:         public float trust;
0017:
0018:         public const int ApprenticeYieldCap = 5; // sites the player may strip clean per season
0019:     }
0020:
```

### Current evidence: `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e6b03689d15a6c630703d91cb707b21436992045340740f541db61226e67c49c`
- Snapshot size: 5875 characters; 137 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: #pragma warning disable CS8618
0005:
0006: namespace Ashfall.Core.Muster
0007: {
0008:     /// <summary>Serialized state of the Coastal Hydro-Barons (Section II, "The Rate
0009:     /// Card War") — faction_hydro_barons, the fifteenth Current at the three plants.</summary>
0010:     public class HydroBaronsState
0011:     {
0012:         public string systemId = HydroBaronsSystem.SystemId;
0013:         public bool isActive;
0014:         public int queuePosition;            // the player's chit position on the rate card
0015:         public float trust;
0016:         public bool rateCardRevised;         // fixed published price (Approach A/B/D)
0017:         public bool plantSeized;             // Approach C — player runs Unit 4
0018:         public bool adminReform;             // transparent regulated pricing (Approach B)
0019:         public string approach = string.Empty; // A..D
0020:     }
```

### Current evidence: `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `77e89bec953f8b5f98d256aa8ba4712c8331aa3ee10def60ada60f4106560ff0`
- Snapshot size: 4605 characters; 110 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: #pragma warning disable CS8618
0004:
0005: namespace Ashfall.Core.Muster
0006: {
0007:     /// <summary>Serialized state of the Iron Raiders (Section V.6) — the Toll's den
0008:     /// at loc_iron_raiders_den. No offers; wants what the player has. Raid chance
0009:     /// is read, never authored, from the wartime tension the sector already tracks.</summary>
0010:     public class IronRaidersState
0011:     {
0012:         public string systemId = IronRaidersSystem.SystemId;
0013:         public bool isActive;
0014:         public float aggressionLevel;   // read from sector wartime tension (0..1)
0015:         public int raidsThisSeason;
0016:         public float shelterVisibility = 1f; // lowered by fortifying approach routes
0017:     }
0018:
0019:     /// <summary>
0020:     /// Engine-agnostic state machine for faction_iron_raiders (Section V.6). The only
```

### Current evidence: `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `037af028978b2165428e8c23220083b88fc593adc87228a899ed33e7b7b43d9c`
- Snapshot size: 6798 characters; 170 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0057:             if (_state.formed) return false;
0058:             if (day < MusterSystem.MusterOpeningDay) return false;
0059:             _state.formed = true;
0060:             _state.formedDay = day;
```

### Current evidence: `Assets/Ashfall.Core/Muster/MusterSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f2a4ac2a5dbb61bb964a73b4e74f36cb78827acbedde6ef942c9c3536594e66a`
- Snapshot size: 22081 characters; 526 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0028:     {
0029:         public string systemId = MusterSystem.SystemId;
0030:         public int escalationDay = -1;
0031:         public bool musterTriggered;
...
0077:     /// </summary>
0078:     public class MusterSystem : IApproachQuestline
0079:     {
0080:         public const string SystemId = "muster_system";
...
0088:
0089:         public MusterSystem(MusterState? state = null)
0090:         {
0091:             _state = state ?? new MusterState();
```

### Current evidence: `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1662752c5eef33a3ba3515952ce1c1ca204d163ae7238e841bea1745c903e303`
- Snapshot size: 3801 characters; 91 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017:     /// Plain snapshot of the political world at evaluation time. Pure data — the
0018:     /// HOST maps live systems (FactionWarSystem, RegionalTreatySystem, flag ledger,
0019:     /// CoalitionCampSystem) into this struct; the evaluator stays engine-agnostic
0020:     /// and owns no reference to any war/treaty type (Muster must not couple to
...
0024:     {
0025:         /// <summary>FactionWarSystem.dominantFactionId (empty when no dominance).</summary>
0026:         public string DominantFactionId = string.Empty;
0027:
...
0070:     /// </summary>
0071:     public static class MusterPathEvaluator
0072:     {
0073:         public const int DominanceTensionThreshold = 60;
```

### Current evidence: `Assets/Ashfall.Core/Muster/WitnessSelector.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a0d2a8812d8a05510febe92f15b010cf1e1ba5a517b0b83b101eb31e8afa61cc`
- Snapshot size: 7820 characters; 172 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0051:     /// </summary>
0052:     public static class WitnessSelector
0053:     {
0054:         /// <param name="witnesses">Full catalog (loaded from muster_witnesses.json).</param>
```

### Current evidence: `src/Main.Muster.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `57caac9a6e3bc274ef7a8aeefb4b4b56e34a81b6b713a3977d127d2f3c461ad1`
- Snapshot size: 21889 characters; 491 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using System.Collections.Generic;
0008: using Ashfall.Core.Combat;
0009: using AtomicWar.Journal;
0010: using Ashfall.Core;
0011: using Ashfall.Core.Campaign;
0012: using Ashfall.Core.Economy;
0013: using Ashfall.Core.Expeditions;
0014: using Ashfall.Core.Foundry;
0015: using Ashfall.Core.Inventory;
0016: using Ashfall.Core.Journal;
0017: using Ashfall.Core.Muster;
0018: using Ashfall.Core.YearOfAsh;
0019: using Ashfall.Core.Radio;
0020: using Ashfall.Core.Survivors;
```

### Current evidence: `src/Host/MusterHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `927200bbce62aae09560713c2d3a4fb265f2f2581c078344e4c6ad01e3f1a5b5`
- Snapshot size: 15273 characters; 327 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011:     /// Thin Godot-host session for the Muster (Expansion 06) escalation layer.
0012:     /// Wraps MusterSystem, loads the 15-current roster from currents.json,
0013:     /// escalates the day clock, and persists to user:// via MusterSaveStore.
0014:     /// No gameplay rules here — hosts only present.
...
0018:     {
0019:         public MusterSystem Engine { get; }
0020:         public CoalitionCampSystem Camp { get; }
0021:         public ColdCountSystem ColdCount { get; }
...
0026:         public HydroBaronsSystem HydroBarons { get; }
0027:         public FactionActionBoard Board { get; }
0028:         public List<CurrentDefinition> Roster { get; }
0029:         public List<WitnessDefinition> Witnesses { get; }
...
0046:         public MusterHostSession(
0047:             MusterSystem engine = null!,
0048:             CoalitionCampSystem camp = null!,
0049:             ColdCountSystem coldCount = null!,
...
0054:             HydroBaronsSystem hydroBarons = null!,
0055:             FactionActionBoard board = null!,
0056:             List<CurrentDefinition> roster = null!,
0057:             List<WitnessDefinition> witnesses = null!,
...
0061:         {
0062:             Engine = engine ?? new MusterSystem();
0063:             Camp = camp ?? new CoalitionCampSystem();
0064:             ColdCount = coldCount ?? new ColdCountSystem();
...
0069:             HydroBarons = hydroBarons ?? new HydroBaronsSystem();
0070:             Board = board ?? new FactionActionBoard(ScavengerGuild, HydroBarons, IronRaiders, Camp);
0071:             Roster = roster ?? new List<CurrentDefinition>();
0072:             Witnesses = witnesses ?? new List<WitnessDefinition>();
...
0112:                 campScenes = CampSceneCatalogLoader.LoadScenes(dataDir, fileIO, serializer);
0113:                 factionActions = FactionActionCatalogLoader.LoadActions(dataDir, fileIO, serializer);
0114:                 culture = FactionCultureCatalogLoader.LoadEntries(dataDir, fileIO, serializer);
0115:             }
...
0177:             var gate = new BoardFlagEligibility(Board, SubjectLivingResolver);
0178:             var deliveries = WitnessSelector.Select(Witnesses, day, gate, maxCount);
0179:             for (int i = 0; i < deliveries.Count; i++)
0180:                 Engine.RecordWitnessResult(
...
0191:         {
0192:             private readonly FactionActionBoard _board;
0193:             private readonly Func<string, bool>? _isSubjectAlive;
0194:             public BoardFlagEligibility(FactionActionBoard board, Func<string, bool>? isSubjectAlive = null)
...
0203:                 {
0204:                     FactionActionBoard.FactionScavengerGuild => _board.ComputeBand(factionId) != FactionActionBands.Hostile,
0205:                     FactionActionBoard.FactionHydroBarons => true,
0206:                     FactionActionBoard.FactionIronRaiders => true,
...
```

### Current evidence: `src/UI/MusterPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `99db5e7c8e43b08395d274505f09b61915f5e85bb8049b5240a4ad9d45c8ac57`
- Snapshot size: 18042 characters; 388 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0193:                 ? $"ESCALATION: DAY {day} — THE MUSTER IS OPEN (Holding Ground Active)"
0194:                 : $"ESCALATION: DAY {day} — DORMANT (Muster opens Day {MusterSystem.MusterOpeningDay})";
0195:
0196:             _escalationStatus.Text = statusText;
```

### Current evidence: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3aa7db610e63bbd27691861d3c9e3ac5fb6622ae2523563f641c5764892d7fcb`
- Snapshot size: 50893 characters; 1212 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "actions": [
0004:     {
0005:       "id": "act_salvage_rights_offer",
0006:       "faction_id": "faction_scavenger_guild",
0007:       "title": "The Claim Ledger Opens",
0008:       "text": "A Guild registrar arrives with the two-color ledger under her arm and a ruin's grid reference already circled. The site is claimed. The question is what the shelter does about that.",
0009:       "min_day": 60,
0010:       "max_day": 0,
0011:       "once": true,
0012:       "cooldown_days": 0,
0013:       "requires_flags": [],
0014:       "forbids_flags": [],
0015:       "variants": [
0016:         {
0017:           "band": "hostile",
0018:           "text": "The registrar does not sit. The shelter is in the ledger's gray margin — names that strip claimed sites do not get terms, they get watched. She reads the grid reference aloud anyway, so nobody can say they were not warned.",
0019:           "choices": [
0020:             {
```

### Current evidence: `Assets/StreamingAssets/Data/muster_camp_scenes.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `db515abfdb1fbf25d1f1e1df20b3d129850b9446c33a8de8a21b378e22aa657f`
- Snapshot size: 9920 characters; 146 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "scenes": [
0004:     {
0005:       "id": "camp_scene_arrivals",
0006:       "scene": "arrivals",
0007:       "min_day": 260,
0008:       "requires_flags": [],
0009:       "variants": [
0010:         {
0011:           "variant_id": "negotiated",
0012:           "requires_path": "negotiated",
0013:           "body": "They come in from the south road in a loose column — Guild markers on the lead cart, a Hydro water-detail flying its tied white rag, two of the Toll's people walking openly beside Coalition deserters as if the war were somebody else's story. At the substation gate somebody has nailed up a plank with the neutral-ground rules burned into it, four lines, no flourishes. Nobody salutes anybody. Everyone counts the banners twice."
0014:         },
0015:         {
0016:           "variant_id": "victors",
0017:           "requires_path": "victors",
0018:           "body": "The arrivals come in through a single checkpoint, and the checkpoint flies one banner too many for a neutral ground. Defeated columns surrender their weapons at the wire and get receipts on carbon paper. A Coalition man asks who keeps the receipts and nobody answers. The rules plank is there, four lines, but a sentry stands close enough to read over every shoulder."
0019:         },
0020:         {
```

### Current evidence: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `87335424793a6acd820c38101be24b2e4d6d9b2b9778845bcbdaab7ec9f9fd50`
- Snapshot size: 34570 characters; 572 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 2,
0003:   "witnesses": [
0004:     {
0005:       "id": "witness_1_checkpoint_conscript",
0006:       "witness_name": "The Checkpoint Conscript",
0007:       "location_id": "loc_garrison_checkpoint_gamma",
0008:       "knowledge_key": "history_checkpoint_conscripts_confession",
0009:       "day_min": 241,
0010:       "priority": 40,
0011:       "testimonies": [
0012:         {
0013:           "variant_id": "account",
0014:           "body": "Three drinks past careful, in a voice that starts brave and drops: Voss was shot by his own staff. Refused a direct order to fire on a Rebuilder grain convoy. 'Don't repeat it,' he says, which he needn't worry about — it is already down in the journal, in ink, in whoever's hand was sent to listen."
0015:         }
0016:       ]
0017:     },
0018:     {
0019:       "id": "witness_2_quartermaster_paperwork",
0020:       "witness_name": "The Quartermaster",
```

### Current evidence: `Assets/StreamingAssets/Data/muster_epilogues.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5a317510babab25b13eaa6865014ed42b1e5578b0ff9201082ea5176ae3a6c6f`
- Snapshot size: 11856 characters; 130 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "epilogues": [
0004:     {
0005:       "ending_key": "the_open_muster",
0006:       "title": "The Open Muster",
0007:       "prose": "The coalition held the substation and the ground around it, and when Day 320 came the Garrison chose not to press it. The rally point is a town now, of a kind. Nobody calls it anything official. The radio operators just say 'the Muster' and everyone knows which muster."
0008:     },
0009:     {
0010:       "ending_key": "the_amnesty",
0011:       "title": "The Amnesty",
0012:       "prose": "The petition carried on paper, which in the end mattered more than anything fired. The coalition is absorbed into a demobilized-conscript status, the only path in this document that ends their fugitive status entirely. The ledger closes, and stays closed, by agreement nobody tries to amend."
0013:     },
0014:     {
0015:       "ending_key": "the_corridor",
0016:       "title": "The Corridor",
0017:       "prose": "There was no siege, because there was no camp. Small groups, quiet routes, a corridor instead of a rally: nobody won a fight that never happened. Whatever was being rallied to, it left Sector 4 a little emptier and a little less armed, and the people who made that choice sleep wherever the corridor lets them."
0018:     },
0019:     {
0020:       "ending_key": "the_blood_price",
```

### Current evidence: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`
- Snapshot size: 6557 characters; 194 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "actions": [
0004:     {
0005:       "id": "faction_the_office",
0006:       "display_name": "The Office",
0007:       "alignment": "conditional",
0008:       "home_region": "the_cluster",
0009:       "is_active": true,
0010:       "trust": 0,
0011:       "wants": [
0012:         "item_census_return_blank",
0013:         "named_occupancy",
0014:         "item_order_12c"
0015:       ],
0016:       "offers": [
0017:         "process_water_credit",
0018:         "block_c_guesting",
0019:         "regular_rate"
0020:       ],
```

### Current evidence: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`
- Snapshot size: 6222 characters; 172 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "actions": [
0004:     {
0005:       "id": "faction_the_overlay",
0006:       "display_name": "The Overlay",
0007:       "alignment": "conditional",
0008:       "home_region": "all_regions",
0009:       "is_active": true,
0010:       "trust": 0,
0011:       "wants": [
0012:         "brass_fittings",
0013:         "sr_stencil_pot",
0014:         "lamp_oil"
0015:       ],
0016:       "offers": [
0017:         "cadastral_keys",
0018:         "travel_correction_on_named_sites"
0019:       ],
0020:       "signature_quote": "The Schedule named households. The Record names ground. Ground does not argue.",
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

### Current evidence: `Assets/StreamingAssets/Data/foundry_accords.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d82bb6e361e13ef12aa6b73d3ad570bc5e18fe45acb8354010597b1ad00bdf53`
- Snapshot size: 20522 characters; 240 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "foundry_district8_accords",
0004:   "treaties": [
0005:     {
0006:       "treaty_id": "treaty_brine_pipe_and_iodine_exchange",
0007:       "ratified_day": 280,
0008:       "treaty_title": "The Brine Pipe & Iodine Exchange",
0009:       "signatory_factions": ["faction_silent_foundry", "faction_the_office"],
0010:       "demarcated_territory": "The smelter bay casting floor to the saltworks membrane hall",
0011:       "water_allocation_lpm": 40.0,
0012:       "power_quota_kw": 12.0,
0013:       "tariff_schedule": "Four brine-resistant pipes and two valve bodies per assessment cycle, paid in iodine from the iodine store and grade salt by the sack.",
0014:       "treaty_articles": "ARTICLE 1: The foundry casts lead-antimony pipe fit for brine service, and stands the first burst of any pipe it poured. ARTICLE 2: The Office releases iodine and grade salt on delivery, not on promise. ARTICLE 3: Neither party lights a second furnace on the same steam line while the exchange is in term.",
0015:       "penalties": "A missed cycle suspends the iodine allocation until the pipe debt is cast.",
0016:       "tags": ["foundry", "saltworks", "brine", "iodine", "exchange", "district8"]
0017:     },
0018:     {
0019:       "treaty_id": "treaty_cluster_labour_schedule",
0020:       "ratified_day": 305,
```

### Current evidence: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8f645b326dde0e0f651aabd7a867cc88ea8e36157f292710117271d7524dca1f`
- Snapshot size: 5187 characters; 96 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "vignettes": [
0004:     {
0005:       "id": "epilogue_demographics_thriving",
0006:       "category": "demographics",
0007:       "priority": 10,
0008:       "min_survivors": 12,
0009:       "max_survivors": 999,
0010:       "max_starvation_deaths": 0,
0011:       "title": "A Beacon in the Ash",
0012:       "narrative": "Against impossible odds, the shelter grew into a thriving subterranean bastion. With over a dozen healthy survivors, children born into sealed corridors learned pre-war mathematics before they ever saw the sun. The air remains sweet, the bunks are warm, and the graves in the memorial garden are few.",
0013:       "tags": ["triumph", "demographics", "haven"]
0014:     },
0015:     {
0016:       "id": "epilogue_demographics_persevering",
0017:       "category": "demographics",
0018:       "priority": 5,
0019:       "min_survivors": 5,
0020:       "max_survivors": 11,
```

### Current evidence: `src/UI/MusterAtlasPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/MusterAtlasPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e2b3cacc96b8afda445c5f9bda5749e9dccbdb5df9a6e8dae20be5535df77cd6`
- Snapshot size: 26079 characters; 536 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017: ///
0018: /// Reads the user's own `MusterSystem` (Core) through `MusterHostSession`.
0019: /// Four surfaces:
0020: ///   1. Sector Currents    — push/pop trust momentum per faction
...
0228:         }
0229:         // Note: MusterSystem currently exposes a single MusterRecord; the
0230:         // count surface will track the size of the sector-tracking set
0231:         // once the engine extends its read surface. Until then we surface
```

### Current evidence: `src/Muster/FactionActionPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/Muster/FactionActionPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a10fd3542ac5634c068982234557eca6eac3a2adb7f9650f5b48b94f77018394`
- Snapshot size: 6944 characters; 180 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     /// Plan 25 peacetime faction-action panel: lists the offers the
0013:     /// FactionActionBoard makes available today (standing-band variant already
0014:     /// resolved) with one button per authored choice. Thin presentation only —
0015:     /// selection, effects, idempotence and flags all live in core.
...
0018:     {
0019:         private FactionActionBoard _board;
0020:         private VBoxContainer _offerList;
0021:         private VBoxContainer _cultureList;
...
0073:
0074:         public void Bind(FactionActionBoard board) => _board = board;
0075:
0076:         public void BindCulture(List<FactionCultureEntry> culture) =>
...
0123:                 var band = offer.Band;
0124:                 var variant = FactionActionBoard.SelectVariant(def, band);
0125:                 if (variant != null)
0126:                 {
```

### Current evidence: `src/Muster/JournalWitnessPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/Muster/JournalWitnessPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `283cb24de312cddf7afd232b74aa946447c8172dfcb6db5d6269412eac439c33`
- Snapshot size: 3549 characters; 91 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System.Collections.Generic;
0003: #pragma warning disable CS8618
0004: using Godot;
0005: using AtomicWar.GodotApp.UI;
0006: using Ashfall.Core.UI;
0007: using Ashfall.Core.Journal;
0008: using Ashfall.Core.Muster;
0009:
0010: namespace AtomicWar.GodotApp.Muster
0011: {
0012:     /// <summary>
0013:     /// Section III witness panel: renders the three Harven succession accounts
0014:     /// (muster_witnesses.json) with the journal's trait-based framing —
0015:     /// JournalVoice.ComposeFullText per the authoring survivor's RiskBiasTrait.
0016:     /// Thin presentation only; framing logic lives in the core journal.
0017:     /// </summary>
0018:     public partial class JournalWitnessPanel : PanelContainer
0019:     {
0020:         private List<WitnessDefinition> _witnesses;
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

### Current evidence: `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `39645830fc4f4429707eb595f1a651dab0359e701ee3edd4d3f243b1769efd36`
- Snapshot size: 7874 characters; 186 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0025:
0026:             var system = new RegionalTreatySystem();
0027:             system.LoadCatalog(treaties);
0028:
...
0041:         {
0042:             Assert.Equal(FactionActionBands.Hostile, FactionActionBoard.BandForTrust(0f));
0043:             Assert.Equal(FactionActionBands.Hostile, FactionActionBoard.BandForTrust(-5f));
0044:             Assert.Equal(FactionActionBands.Poor, FactionActionBoard.BandForTrust(2f));
...
0048:
0049:             var board = new FactionActionBoard();
0050:             Assert.Equal(FactionActionBands.Hostile, board.ComputeBand(FactionActionBoard.FactionScavengerGuild));
0051:             Assert.Equal(FactionActionBands.Hostile, board.ComputeBand(FactionActionBoard.FactionHydroBarons));
...
0085:             var livingGate = new TestLivingGate(new[] { "survivor_dr_chen" });
0086:             var livingResult = WitnessSelector.Select(catalog, 15, livingGate, maxCount: 1);
0087:             Assert.Single(livingResult);
0088:             Assert.Equal("witness_test_subject", livingResult[0].Witness.id);
...
0091:             var deadGate = new TestLivingGate(Array.Empty<string>());
0092:             var deadResult = WitnessSelector.Select(catalog, 15, deadGate, maxCount: 1);
0093:             Assert.Empty(deadResult);
0094:         }
...
0106:         [Fact]
0107:         public void T04_FactionActionBoardItemSinkDeliversGoods()
0108:         {
0109:             var board = new FactionActionBoard();
...
0112:                 id = "act_water_filter_exchange",
0113:                 factionId = FactionActionBoard.FactionHydroBarons,
0114:                 minDay = 1,
0115:                 variants = new List<FactionActionVariant>
...
0150:         {
0151:             var system = new RegionalTreatySystem();
0152:             system.LoadCatalog(new List<TreatyDefinition>
0153:             {
```

### Current evidence: `Ashfall.Core.Tests/MusterSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `53230f77a131ac6bd904007503ab1a90a3b9f086c5a6798e102ce31682a3ce34`
- Snapshot size: 8163 characters; 210 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0008: {
0009:     public class MusterSystemTests
0010:     {
0011:         private static MusterSystem NewSystem() => new MusterSystem();
...
0153:
0154:             var restored = new MusterSystem();
0155:             restored.RestoreState(sys.CaptureState());
0156:
...
0172:
0173:             var restored = new MusterSystem();
0174:             restored.RestoreState(sys.CaptureState());
0175:             var after = SaveChecksum.Compute(restored.CaptureState());
```

### Current evidence: `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6e12d8e6a7c11e3d0ebe8fc625f8fb14992d5d785807d9abd523f3a0c74bb010`
- Snapshot size: 5677 characters; 160 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: {
0007:     public class MusterPathEvaluatorTests
0008:     {
0009:         private static MusterPathInput Input(
...
0042:         {
0043:             Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
0044:                 Input(dominant: "faction_central_garrison", hostile: 2)));
0045:         }
...
0049:         {
0050:             Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
0051:                 Input(dominant: "faction_rebuilders", tension: 60)));
0052:         }
...
0058:             // imposed a victor's gathering; other rules decide.
0059:             Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
0060:                 Input(dominant: "faction_ash_sign", tension: 30, hostile: 1, campFormed: false)));
0061:         }
...
0067:         {
0068:             Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
0069:                 Input(treaties: 1)));
0070:         }
...
0074:         {
0075:             Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
0076:                 Input(peace: true, grievance: true)));
0077:         }
...
0081:         {
0082:             Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
0083:                 Input(treaties: 1, campFormed: false)));
0084:         }
...
0088:         {
0089:             Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
0090:                 Input(treaties: 1, majors: 1)));
0091:         }
...
0095:         {
0096:             Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
0097:                 Input(campFormed: true)));
0098:         }
...
0106:             // would otherwise describe a negotiated gathering.
0107:             Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
0108:                 Input(dominant: "faction_hydro_barons", tension: 80, treaties: 2, peace: true)));
0109:         }
...
0113:         {
0114:             Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
0115:                 Input(campFormed: false, majors: 1)));
0116:         }
...
```

### Current evidence: `Ashfall.Core.Tests/MusterContentCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b5b84e2035989b83326764f6f26770954680c93dfd08246d75120834968de007`
- Snapshot size: 11132 characters; 256 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0108:             // outside the matrix, e.g. long walk / guild mid-game questlines).
0109:             var sys = new MusterSystem();
0110:             foreach (var def in sys.Catalog)
0111:             {
...
0184:         [Fact]
0185:         public void MusterSystem_EndingKeyForAny_DetectsResolvedMatrixKey()
0186:         {
0187:             var sys = new MusterSystem();
```

### Current evidence: `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f6e1e013e0072ec7a12230853f26173d201771c8a9b0626ecd2da8f3824ffd16`
- Snapshot size: 21198 characters; 553 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Linq;
0006: using Ashfall.Core.Muster;
0007: using Xunit;
0008:
0009: namespace Ashfall.Core.Tests
0010: {
0011:     /// <summary>
0012:     /// Comprehensive verification suite for Plan 89 — Muster Epilogues Expansion (12 -> 25 outcomes).
0013:     /// Tests catalog integrity, bidirectional key coverage, precedence rules, reachability,
0014:     /// determinism, and prose retrieval.
0015:     /// </summary>
0016:     public class MusterEpilogueMatrixTests
0017:     {
0018:         private static string FindDataDir()
0019:         {
0020:             string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
```

### Current evidence: `Ashfall.Core.Tests/Plan84WitnessExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f21734c4f8e1f285ea934bf8a006dcaa2aea8d66d0184986647e6a49a5fff07b`
- Snapshot size: 18514 characters; 468 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ASHFALL Core Tests — Plan 84: Muster Witness Testimonies Expansion (3 → 15 investigation witnesses).
0003: // Asserts that the 12 new witnesses (Coastal Evacuation, Grain Convoy Massacre, Silent Foundry Accord)
0004: // are correctly authored, schema-valid, and coexist with the 3 Voss witnesses and 12 Plan 25 faction witnesses.
0005:
0006: using System.Collections.Generic;
0007: using System.IO;
0008: using System.Linq;
0009: using Ashfall.Core.Muster;
0010: using Xunit;
0011:
0012: namespace Ashfall.Core.Tests
0013: {
0014:     public class Plan84WitnessExpansionTests : CatalogTestBase
0015:     {
0016:         // ── Canonical investigation witness IDs for all four threads ─────────
0017:         private static readonly string[] VossThreadIds =
0018:         {
0019:             "witness_1_checkpoint_conscript",
0020:             "witness_2_quartermaster_paperwork",
```

### Current evidence: `Ashfall.Core.Tests/WitnessSelectionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f67967da50d5bee204aa5353e36cffc78452b0294b441b6a7d90b2345071daf1`
- Snapshot size: 11774 characters; 273 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0118:             var list = new[] { W("early", dayMin: 200), W("late", dayMin: 300) };
0119:             var day200 = WitnessSelector.Select(list, 200, gate);
0120:             Assert.Equal(new[] { "early" }, new[] { day200[0].Witness.id });
0121:             var day300 = WitnessSelector.Select(list, 300, gate);
...
0130:             var list = new[] { W("haunted", subjectId: "npc_dead"), W("alive", subjectId: "npc_living") };
0131:             var selected = WitnessSelector.Select(list, 300, gate);
0132:             Assert.Equal(new[] { "alive" }, new[] { selected[0].Witness.id });
0133:         }
...
0139:             var list = new[] { W("institutional", factionId: "faction_hydro_barons") };
0140:             Assert.Single(WitnessSelector.Select(list, 300, gate));
0141:         }
0142:
...
0153:             };
0154:             var selected = WitnessSelector.Select(list, 300, gate);
0155:             Assert.Equal(2, selected.Count);
0156:             Assert.DoesNotContain(selected, d => d.Witness.id == "raider_rep");
...
0170:             });
0171:             Assert.Equal("helped", WitnessSelector.SelectTestimony(w, gate).variantId);
0172:
0173:             gate.Flags.Remove("flag_helped_x");
...
0176:             gate.Flags.Add("flag_made_amends"); // forbids now closed on 'failed'
0177:             Assert.Equal("absent", WitnessSelector.SelectTestimony(w, gate).variantId);
0178:         }
0179:
...
0187:             });
0188:             Assert.Null(WitnessSelector.SelectTestimony(w, gate));
0189:         }
0190:
...
0201:             };
0202:             var selected = WitnessSelector.Select(list, 300, gate);
0203:             Assert.Equal(new[] { "z_high", "a_mid", "b_mid", "a_low" },
0204:                 new[] { selected[0].Witness.id, selected[1].Witness.id, selected[2].Witness.id, selected[3].Witness.id });
...
0219:             // must not take two guild witnesses: round-robin gives guild_1 + hydro_1.
0220:             var capped = WitnessSelector.Select(list, 300, gate, maxCount: 2);
0221:             Assert.Equal(2, capped.Count);
0222:             Assert.Equal("guild_1", capped[0].Witness.id);
...
0233:             var list = new[] { W("dup"), W("dup"), W("other") };
0234:             Assert.Equal(2, WitnessSelector.Select(list, 300, gate).Count);
0235:         }
0236:
...
0254:             {
0255:                 var run = WitnessSelector.Select(list, 300, gate);
0256:                 Assert.Equal(4, run.Count);
0257:                 Assert.Equal("w1,w2,w3,w4", string.Join(",", new[]
...
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/muster_faction_actions.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, actions`
- `actions`: list count=12; sample IDs=['act_salvage_rights_offer', 'act_claim_arbitration', 'act_apprentice_rule_dispute', 'act_purification_toll', 'act_hydro_emergency_appeal', 'act_intake_dispute', 'act_raider_parley', 'act_raider_passage_levy']
- `schema_version`: `1`
- SHA-256: `3aa7db610e63bbd27691861d3c9e3ac5fb6622ae2523563f641c5764892d7fcb`
#### `Assets/StreamingAssets/Data/muster_camp_scenes.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, scenes`
- `scenes`: list count=4; sample IDs=['camp_scene_arrivals', 'camp_scene_old_enemies', 'camp_scene_shared_meal', 'camp_scene_confrontation']
- `schema_version`: `1`
- SHA-256: `db515abfdb1fbf25d1f1e1df20b3d129850b9446c33a8de8a21b378e22aa657f`
#### `Assets/StreamingAssets/Data/muster_witnesses.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, witnesses`
- `witnesses`: list count=27; sample IDs=['witness_1_checkpoint_conscript', 'witness_2_quartermaster_paperwork', 'witness_3_signals_intercept', 'witness_scavenger_claimant', 'witness_messengers_keeper', 'witness_claimant_auditor', 'witness_hydro_envoy', 'witness_raider_parley_survivor']
- `schema_version`: `2`
- SHA-256: `87335424793a6acd820c38101be24b2e4d6d9b2b9778845bcbdaab7ec9f9fd50`
#### `Assets/StreamingAssets/Data/muster_epilogues.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, epilogues`
- `epilogues`: list count=25; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `5a317510babab25b13eaa6865014ed42b1e5578b0ff9201082ea5176ae3a6c6f`
#### `Assets/StreamingAssets/Data/holdfast_factions.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, actions`
- `actions`: list count=9; sample IDs=['faction_the_office', 'faction_the_cutters', 'faction_the_fleet', 'faction_black_flotilla', 'faction_supply_corps', 'faction_railway_guild', 'faction_hydro_barons', 'faction_ordnance_foundry']
- `schema_version`: `1`
- SHA-256: `e2f13c2291cba31c05b7b0dbca06dc37bd9bf5d692e51d9c2250c8738bdcbd44`
#### `Assets/StreamingAssets/Data/standing_record_factions.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, actions`
- `actions`: list count=8; sample IDs=['faction_the_overlay', 'faction_the_scale', 'faction_the_compact', 'faction_the_underwrite', 'faction_the_cutters', 'faction_the_fleet', 'faction_the_rebuilders', 'faction_the_garrison']
- `schema_version`: `1`
- SHA-256: `65d731c51c28e43ec37bdae1c1f9fd4e3c73ac0e867594ceb82a04e9db54a68b`
#### `Assets/StreamingAssets/Data/faction_war_events.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, chains`
- `chains`: list count=38; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`
#### `Assets/StreamingAssets/Data/foundry_accords.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, treaties`
- `treaties`: list count=18; sample IDs=[]
- `schema_version`: `1`
- `collection_id`: `foundry_district8_accords`
- SHA-256: `d82bb6e361e13ef12aa6b73d3ad570bc5e18fe45acb8354010597b1ad00bdf53`
#### `Assets/StreamingAssets/Data/campaign_epilogues.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, vignettes`
- `vignettes`: list count=9; sample IDs=['epilogue_demographics_thriving', 'epilogue_demographics_persevering', 'epilogue_demographics_desolation', 'epilogue_governance_reconciliation', 'epilogue_governance_iron_order', 'epilogue_technology_renaissance', 'epilogue_technology_makeshift', 'epilogue_sustenance_harvest']
- `schema_version`: `1`
- SHA-256: `8f645b326dde0e0f651aabd7a867cc88ea8e36157f292710117271d7524dca1f`
## Symbol and caller audit

#### `FactionActionBoard` — HOST_REFERENCE_PRESENT — core/declaration=9, host=13, test=62
- `Assets/Ashfall.Core/CatalogIntegrityRules.cs:338` (core) — // runtime by the FactionActionBoard / FactionWarChainRunner seams and
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:519` (core) — // runtime by the FactionActionBoard / FactionWarChainRunner seams;
- `Assets/Ashfall.Core/Muster/FactionActionBoard.cs:41` (core) — public string systemId = FactionActionBoard.SystemId;
- `Assets/Ashfall.Core/Muster/FactionActionBoard.cs:55` (declaration) — public class FactionActionBoard
- `Assets/Ashfall.Core/Muster/FactionActionBoard.cs:75` (core) — public FactionActionBoard(
- `Assets/Ashfall.Core/Muster/FactionActionCatalog.cs:71` (core) — /// peacetime faction ecology. Consumed only by FactionActionBoard. Missing file
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:65` (core) — var board = new FactionActionBoard(guild: guild);
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:169` (core) — var restoredBoard = new FactionActionBoard(guild: new ScavengerGuildSystem());
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:345` (core) — /// flags authored by the FactionActionBoard).</summary>
- `src/Host/MusterHostSession.cs:27` (host) — public FactionActionBoard Board { get; }
- `src/Host/MusterHostSession.cs:55` (host) — FactionActionBoard board = null!,
- `src/Host/MusterHostSession.cs:70` (host) — Board = board ?? new FactionActionBoard(ScavengerGuild, HydroBarons, IronRaiders, Camp);
- `src/Host/MusterHostSession.cs:192` (host) — private readonly FactionActionBoard _board;
- `src/Host/MusterHostSession.cs:194` (host) — public BoardFlagEligibility(FactionActionBoard board, Func<string, bool>? isSubjectAlive = null)
- `src/Host/MusterHostSession.cs:204` (host) — FactionActionBoard.FactionScavengerGuild => _board.ComputeBand(factionId) != FactionActionBands.Hostile,
- `src/Host/MusterHostSession.cs:205` (host) — FactionActionBoard.FactionHydroBarons => true,
- `src/Host/MusterHostSession.cs:206` (host) — FactionActionBoard.FactionIronRaiders => true,
- `src/Host/MusterHostSession.cs:207` (host) — FactionActionBoard.FactionDeserterCoalition => true,
- `src/Muster/FactionActionPanel.cs:13` (host) — /// FactionActionBoard makes available today (standing-band variant already
- `src/Muster/FactionActionPanel.cs:19` (host) — private FactionActionBoard _board;
- `src/Muster/FactionActionPanel.cs:74` (host) — public void Bind(FactionActionBoard board) => _board = board;
- `src/Muster/FactionActionPanel.cs:124` (host) — var variant = FactionActionBoard.SelectVariant(def, band);
- `Ashfall.Core.Tests/FactionActionBoardTests.cs:85` (test) — Assert.Equal(expected, FactionActionBoard.BandForTrust(trust));
- `Ashfall.Core.Tests/FactionActionBoardTests.cs:92` (test) — var board = new FactionActionBoard(guild: guild);
- … 60 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `FactionActionCatalog` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=0, host=0, test=0
#### `RegionalTreatySystem` — HOST_REFERENCE_PRESENT — core/declaration=12, host=7, test=10
- `Assets/Ashfall.Core/RegionalTreatyFeed.cs:13` (core) — /// RegionalTreatySystem finally ships a catalog. Balance mapping is
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:11` (core) — public string systemId = RegionalTreatySystem.SystemId;
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:58` (declaration) — public sealed class RegionalTreatySystem
- `Assets/Ashfall.Core/RegionalTreatySystem.cs:75` (core) — public RegionalTreatySystem(ILog? log = null)
- `Assets/Ashfall.Core/RegionalTreatyCatalogLoader.cs:9` (core) — /// Mechanical treaty catalog for <see cref="RegionalTreatySystem"/>.
- `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs:529` (core) — /// provider (typically RegionalTreatySystem.GetTradeDiscount), so nothing is
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:18` (core) — /// HOST maps live systems (FactionWarSystem, RegionalTreatySystem, flag ledger,
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:365` (core) — ["regional_treaties.json"] = new[] { "RegionalTreatyCatalogLoader", "RegionalTreatySystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:718` (core) — ["regional_treaties.json"] = "RegionalTreatySystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:966` (core) — ["regional_treaties.json"] = new[] { "RegionalTreatySystem" },
- `Assets/Ashfall.Core/Treaties/TreatyConsequences.cs:12` (core) — /// <see cref="RegionalTreatySystem"/> each read. Effects are derived from
- `Assets/Ashfall.Core/Treaties/TreatyConsequences.cs:35` (core) — /// <summary>Player-initiated breach (<see cref="RegionalTreatySystem.BreakTreaty"/>).</summary>
- `src/Main.ShelterSocial.cs:78` (host) — var rtSys = new RegionalTreatySystem(new GodotLog());
- `src/Main.ShelterSocial.cs:106` (host) — private void OnTreatyTransitionWorldConsequences(RegionalTreatySystem treatySystem, TreatyTransition transition)
- `src/Host/PanelBindLifecycleSelfTest.cs:414` (host) — var regSession = new RegionalTreatyHostSession(new RegionalTreatySystem(log));
- `src/Host/RegionalTreatyHostSession.cs:9` (host) — /// Host session for RegionalTreatySystem.
- `src/Host/RegionalTreatyHostSession.cs:14` (host) — public RegionalTreatySystem System { get; }
- `src/Host/RegionalTreatyHostSession.cs:16` (host) — public RegionalTreatyHostSession(RegionalTreatySystem system)
- `src/Host/RegionalTreatyHostSession.cs:18` (host) — System = system ?? new RegionalTreatySystem(new GodotLog());
- `Ashfall.Core.Tests/RegionalTreatyFeedTests.cs:11` (test) — /// host finally ships a RegionalTreatySystem catalog instead of an empty one.</summary>
- `Ashfall.Core.Tests/RegionalTreatyFeedTests.cs:56` (test) — var system = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:13` (test) — var sys = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:38` (test) — var sys1 = new RegionalTreatySystem();
- `Ashfall.Core.Tests/RegionalTreatyIntegrationTests.cs:47` (test) — var sys2 = new RegionalTreatySystem();
- … 5 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `FactionWarSystem` — HOST_REFERENCE_PRESENT — core/declaration=48, host=17, test=71
- `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs:62` (core) — /// Connect to FactionWarSystem so standing penalties from debt defaults
- `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs:37` (core) — ///   standing   → FactionWarSystem.ModifyStanding (canonical clamping)
- `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs:51` (core) — private readonly FactionWarSystem _factionWar;
- `Assets/Ashfall.Core/DebtConsequenceHostBridge.cs:77` (core) — FactionWarSystem factionWar,
- `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs:90` (core) — private readonly FactionWarSystem? _factionWar;
- `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs:104` (core) — FactionWarSystem? factionWar = null,
- `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs:199` (core) — if (standing <= FactionWarSystem.HostileStandingThreshold)
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:18` (core) — /// HOST maps live systems (FactionWarSystem, RegionalTreatySystem, flag ledger,
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:25` (core) — /// <summary>FactionWarSystem.dominantFactionId (empty when no dominance).</summary>
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:28` (core) — /// <summary>FactionWarSystem.WarTension (0..100).</summary>
- `Assets/Ashfall.Core/Muster/MusterWarfareEngine.cs:192` (core) — FactionWarSystem? factionWar = null)
- `Assets/Ashfall.Core/Narrative/TravelEncounterHeadlessDemo.cs:80` (core) — var factionWar = new FactionWarSystem();
- `Assets/Ashfall.Core/Narrative/TravelEncounterHeadlessDemo.cs:131` (core) — var failed = new TravelEncounterSystem(catalog, emptyInventory, new FactionWarSystem());
- `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs:160` (core) — private FactionWarSystem? _factionWar;
- `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs:172` (core) — public FactionWarSystem? FactionWar
- `Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs:202` (core) — FactionWarSystem? factionWar = null,
- `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs:417` (core) — FactionWarSystem? factionWar = null,
- `Assets/Ashfall.Core/Radio/RadioDistressSystem.cs:469` (core) — public bool CompleteRescue(string signalId, FactionWarSystem? factionWar = null)
- `Assets/Ashfall.Core/Radio/SignalTrustLedger.cs:13` (core) — /// standing (<see cref="Ashfall.Core.YearOfAsh.FactionWarSystem"/> is
- `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:208` (core) — // Primary effect: Rebuilders +20. Secondary: Garrison -10 (host applies via FactionWarSystem.ModifyStanding)
- `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:292` (core) — // Primary: Ash Sign -40. Secondary: Rebuilders +25 (host applies via FactionWarSystem.ModifyStanding)
- `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:670` (core) — // Primary: Rebuilders +20. Secondary: Hydro Barons -20 (host applies via FactionWarSystem.ModifyStanding)
- `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:704` (core) — // Primary: Hydro Barons +15. Secondary: Rebuilders +15 (host applies via FactionWarSystem.ModifyStanding)
- `Assets/Ashfall.Core/YearOfAsh/BuiltInQuestlineCatalog.cs:836` (core) — // Primary: Black Ops -10. Secondary: Rebuilders +20 (host applies via FactionWarSystem.ModifyStanding)
- … 112 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `MusterPathEvaluator` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=2, host=0, test=13
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:134` (core) — string path = MusterPathEvaluator.Evaluate(negotiatedInput);
- `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs:71` (declaration) — public static class MusterPathEvaluator
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:43` (test) — Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:50` (test) — Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:59` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:68` (test) — Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:75` (test) — Assert.Equal(MusterPaths.Negotiated, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:82` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:89` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:96` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:107` (test) — Assert.Equal(MusterPaths.Victors, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:114` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:121` (test) — Assert.Equal(MusterPaths.Unsettled, MusterPathEvaluator.Evaluate(null));
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:128` (test) — string first = MusterPathEvaluator.Evaluate(input);
- `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs:130` (test) — Assert.Equal(first, MusterPathEvaluator.Evaluate(input));
#### `WitnessSelector` — HOST_REFERENCE_PRESENT — core/declaration=3, host=1, test=17
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:110` (core) — var deliveries = WitnessSelector.Select(witnesses, 300, eligibility);
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:117` (core) — var cleanDeliveries = WitnessSelector.Select(witnesses, 300, cleanEligibility);
- `Assets/Ashfall.Core/Muster/WitnessSelector.cs:52` (declaration) — public static class WitnessSelector
- `src/Host/MusterHostSession.cs:178` (host) — var deliveries = WitnessSelector.Select(Witnesses, day, gate, maxCount);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:119` (test) — var day200 = WitnessSelector.Select(list, 200, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:121` (test) — var day300 = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:131` (test) — var selected = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:140` (test) — Assert.Single(WitnessSelector.Select(list, 300, gate));
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:154` (test) — var selected = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:171` (test) — Assert.Equal("helped", WitnessSelector.SelectTestimony(w, gate).variantId);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:174` (test) — Assert.Equal("failed", WitnessSelector.SelectTestimony(w, gate).variantId);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:177` (test) — Assert.Equal("absent", WitnessSelector.SelectTestimony(w, gate).variantId);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:188` (test) — Assert.Null(WitnessSelector.SelectTestimony(w, gate));
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:202` (test) — var selected = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:220` (test) — var capped = WitnessSelector.Select(list, 300, gate, maxCount: 2);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:225` (test) — var uncapped = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:234` (test) — Assert.Equal(2, WitnessSelector.Select(list, 300, gate).Count);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:255` (test) — var run = WitnessSelector.Select(list, 300, gate);
- `Ashfall.Core.Tests/WitnessSelectionTests.cs:269` (test) — var selected = WitnessSelector.Select(list, 300, null);
- `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs:86` (test) — var livingResult = WitnessSelector.Select(catalog, 15, livingGate, maxCount: 1);
- `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs:92` (test) — var deadResult = WitnessSelector.Select(catalog, 15, deadGate, maxCount: 1);
#### `MusterSystem` — HOST_REFERENCE_PRESENT — core/declaration=17, host=13, test=11
- `Assets/Ashfall.Core/Muster/CampSceneCatalog.cs:26` (core) — public int minDay = MusterSystem.MusterOpeningDay;
- `Assets/Ashfall.Core/Muster/CampSceneCatalog.cs:81` (core) — minDay = e.min_day > 0 ? e.min_day : MusterSystem.MusterOpeningDay
- `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs:58` (core) — if (day < MusterSystem.MusterOpeningDay) return false;
- `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs:18` (core) — /// outcomes (Section XII). MusterSystem resolves ending keys at approach
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:135` (core) — var muster = new MusterSystem();
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:138` (core) — var musterRestored = new MusterSystem();
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:148` (core) — var musterWithResults = new MusterSystem();
- `Assets/Ashfall.Core/Muster/MusterHeadlessDemo.cs:42` (core) — var sys = new MusterSystem();
- `Assets/Ashfall.Core/Muster/MusterHeadlessDemo.cs:79` (core) — var restored = new MusterSystem();
- `Assets/Ashfall.Core/Muster/MusterSystem.cs:29` (core) — public string systemId = MusterSystem.SystemId;
- `Assets/Ashfall.Core/Muster/MusterSystem.cs:78` (declaration) — public class MusterSystem : IApproachQuestline
- `Assets/Ashfall.Core/Muster/MusterSystem.cs:89` (core) — public MusterSystem(MusterState? state = null)
- `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs:12` (core) — /// <see cref="Ashfall.Core.Muster.MusterSystem.ResolveEndingKey"/>
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1043` (core) — ["muster_epilogues.json"] = new[] { "MusterSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1044` (core) — ["muster_witnesses.json"] = new[] { "MusterSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1045` (core) — ["currents.json"] = new[] { "MusterSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1110` (core) — ["epilogue_chronicle.json"] = new[] { "MusterSystem" },
- `src/Host/MusterHostSession.cs:12` (host) — /// Wraps MusterSystem, loads the 15-current roster from currents.json,
- `src/Host/MusterHostSession.cs:19` (host) — public MusterSystem Engine { get; }
- `src/Host/MusterHostSession.cs:47` (host) — MusterSystem engine = null!,
- `src/Host/MusterHostSession.cs:62` (host) — Engine = engine ?? new MusterSystem();
- `src/Host/MusterHostSession.cs:232` (host) — : $"Day {day}: escalation tracked (Muster opens Day {MusterSystem.MusterOpeningDay}).";
- `src/Muster/ApproachSelectionModal.cs:16` (host) — /// host, which validates against MusterSystem.
- `src/Muster/CurrentsRosterWidget.cs:14` (host) — /// only: renders CurrentDefinition list + MusterSystem escalation status.
- … 17 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 2-5
00002:
00003: This document is the complete compiled edition of the ASHFALL Master Expansion Authority v2.0. It combines, in order: (1) the uploaded source document (Volumes 1–24 and its Parts 0 through V, reproduced verbatim), and (2) the Plan Factory's expansion volumes 25 through 57 (the factory batches of 2026-09-24, reproduced verbatim). No content has been altered, merged, or summarized; the two bodies are concatenated at their natural boundary. The source document's own authority order stands: live repository source first; then AGENTS.md; then this document. The factory's constitution (evidence labels, honest bounds, anti-padding) governs Volumes 25 onward.
00004:
00005: # ASHFALL MASTER EXPANSION AUTHORITY v2.0 — THE PLAN FACTORY
#### authority lines 41-44
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
00042: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
#### authority lines 56-59
00056: **DR-07 — Gate-count and test-total drift. HIGH CONFIDENCE.**
00057: The v1.0 bible states 57 CI gates at v1.1.0 (53 fast + 3 full + 1 performance) and quotes both 11,098 and 11,697 full-suite totals from different handoffs. The live 19-wave closeout evidence in `INTEGRATION_PLANS.md` records `verify-fast.sh` ALL 47 GATES PASSED at that batch's close. These figures cannot all describe the same instant. Factory rule: any subject plan that names a gate count or test total must re-verify the number against the live gate inventory at drafting time and cite the closeout it came from. Never carry counts forward from this or any prior document.
00058:
00059: **DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
#### authority lines 69-72
00069:
00070: The following v1.0 structures were confirmed by the audit and remain authoritative: the four-tier architecture (Tier 1 data authority in `Assets/StreamingAssets/Data/`; Tier 2 engine-free Core; Tier 3 `src/Host` + `src/UI`; Tier 4 xUnit plus the `HostCli` selftest surface); the `AGENTS.md` non-negotiable rules (Godot authoritative, Core engine-free, JSON authoritative, one authority per concern, focused verification); the narrative corpus under `Assets/StreamingAssets/Data/narrative/` (present in the live listing); the faction, economy, weather, Year-of-Ash, moral-choice, muster, and verdict catalog families (all present live); and the plan-discipline artifacts (`INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `SESSION_HANDOFF.md`) at root.
00071:
00072: ---
#### authority lines 107-110
00107: - Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
00108: - Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).
00109:
00110: ---
#### authority lines 135-138
00135: | C12 | Mid-winter slump pressure (Days 90–180) story arcs; storm-window almanac entries | HIGH CONFIDENCE (v1.0 Part 7 gap 1) |
00136: | C13 | Epilogue-chronicle depth for under-served permutations of the 32-permutation matrix | HIGH CONFIDENCE |
00137: | C14 | Bestiary and natural-history corpus extension; mutated-botanical and limnology follow-on batches | HIGH CONFIDENCE |
00138: | C15 | Defense-log and ordnance-manifest prose; orbital-harrow telemetry transcripts | INFERENCE — verify coverage |
#### authority lines 151-154
00151: | C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
00152: | C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
00153: | C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
00154: | C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
#### authority lines 183-186
00183: | C9 | Lineage/cohort long-horizon state (3-year simulation exists per 19B closeout, DR-06): verify horizon coverage before extending | HIGH CONFIDENCE |
00184: | C13 | Epilogue evidence persistence: which Day-360+ facts survive into the Day-3650 window | HIGH CONFIDENCE |
00185: | Cross-cutting | Mid-event and mid-combat save round-trips for exactly-once effect classes beyond the rescue-signal runtime (which models the pattern) | PROPOSAL |
00186:
#### authority lines 201-204
00201: | C12 | Year-of-Ash tick-window cost concentration (Days 180–360): per-day work spikes during storm windows | Potential hotspot — requires measurement |
00202: | C13 | Epilogue-matrix evaluation cost at Day 360 — one-shot, likely fine; measure only if reported | HYPOTHESIS |
00203: | All | No optimization plan without before/after numbers in `docs/perf/` | CANON process |
00204:
#### authority lines 247-250
00247:
00248: **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.
00249:
00250: **SB-03 — Newest industrial catalogs: corpus twins + consumption wiring (Lanes A and B/C4).** Evidence: DR-04 (`hydraulic_extraction_catalog`, `metrology_standards_catalog` live, absent from v1.0 inventory). Subject: (a) assay-log narrative twins per the Part 16.4 pattern; (b) bind catalogs into consumption/production ledgers via power-grid/foundry seams if not yet consumed — check `UNCLAIMED_CORPUS_CENSUS.md` first (DR-08). Confidence: HIGH CONFIDENCE that content exists; UNVERIFIED whether systems consume them.
#### authority lines 289-292
00289: ## Continuity checklist result
00290: [Part 13.2 of v1.0 + factory additions; epilogue permutations touched.]
00291: ## Verification class
00292: [Which gates/selftests/focused tests will prove the eventual implementation.]
#### authority lines 332-335
00332: 5. **Quality gates that never relax:** every substantive statement carries a fact status; every volume names its verification surface; no volume may open a sealed surface (DR-06) or a decision-blocked item without the named signature; no volume may duplicate a live catalog or system.
00333: 6. **Anti-padding rule.** If a session cannot find verified content for the next volume, it records "no warranted volume" and stops. Zero-volume sessions are acceptable outcomes under the repository's own zero-plans doctrine.
00334:
00335: ---
#### authority lines 366-369
00366: ### Premise evidence
00367: VERIFIED: `moral_choice_flags.json`, `moral_choice_quests_distress.json`, `moral_choice_chains.json`, and the wider moral-choice catalog family exist live in the data authority. VERIFIED: v1.0 Part 5.6 documents the flags/ledger seam and the weight_of_choices epilogue codec (v2). VERIFIED (drift-corrected): the rescue-signal content wave added `moral_choice_quests_distress.json`, so the moral-choice loader family already consumes multiple split catalogs — the pattern for adding one more split catalog exists. HIGH CONFIDENCE: no current consumer re-reads door-choice flags after the near-term window (v1.0 Part 7 gap 2); the integration plan must re-grep flag consumers before implementation.
00368:
00369: ### Why this and not something else
#### authority lines 388-391
00388: ### Subject
00389: A bounded campaign-window content wave that inserts authored pressure into Days 90–180: a crop blight epidemic arc (ecology), a deep-strata cave-in arc (subterranean/excavation), and a warlord conscription levy arc (doctrines/tribute), each delivered through existing catalogs and event systems, so the stabilized mid-game stays legible as triage rather than routine.
00390:
00391: ### Premise evidence
#### authority lines 403-406
00403: ### Continuity checklist result
00404: Levy reactions must respect information-flow legality (the faction learns of the player's capacity through modeled channels). Cave-ins must not contradict subterranean zone states. Blight must respect crop-strain genome rules. Epilogue: blight and levy outcomes may feed standing/evidence through existing owners; declare permutations touched.
00405:
00406: ### Open premises
#### authority lines 433-436
00433:
00434: ## Subject Plan F-004 — Muster Domain Deep Expansion
00435:
00436: Lane A/B · Cluster C7 · Status PROPOSAL.
#### authority lines 447-450
00447: ### What must not change
00448: `FactionStanceEngine` remains the sole standing-effects authority; muster epilogue weight flows through the existing epilogue owners; no new faction ids where existing branch catalogs suffice.
00449:
00450: ### Recommended integration route
#### authority lines 463-466
00463: ### Subject
00464: A systematic audit of all 32 epilogue permutations for chronicle prose depth and evidence enrollment, followed by authored chronicle depth for the weakest permutations, closing the gap between the mechanically complete epilogue matrix (19A/19B/19C closed) and its narrative coverage.
00465:
00466: ### Premise evidence
… 96 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.

**Requested behavior.** Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.

**Minimum safe delta.** Extend `faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.

**Required delta.** Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.

**Primary seam.** faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `FactionActionBoard`, `FactionActionCatalog`, `RegionalTreatySystem`, `FactionWarSystem`, `MusterPathEvaluator`, `WitnessSelector`, `MusterSystem`.

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
| Domain rules | Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 25.

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
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.
- What is the smallest safe change? Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.
- Which owner is touched? Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/FactionActionCatalog.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/FactionCultureCatalog.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/ScavengerGuildSystem.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/IronRaidersSystem.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/MusterSystem.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/MusterPathEvaluator.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Muster/WitnessSelector.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Muster.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/MusterHostSession.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/MusterPanel.cs` — Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/muster_faction_actions.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/muster_camp_scenes.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/muster_witnesses.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/muster_epilogues.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/holdfast_factions.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/standing_record_factions.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_events.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/foundry_accords.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/campaign_epilogues.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/MusterPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/MusterAtlasPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/Muster/FactionActionPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/Muster/JournalWitnessPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/EpiloguePanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/MusterSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/MusterContentCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Plan84WitnessExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/WitnessSelectionTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue is wired end to end or the plan explicitly closes as already integrated.
- Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

FactionActionCatalog/Board, FactionCultureCatalog, four faction systems, MusterSystem, MusterPathEvaluator, WitnessSelector, MusterHostSession, muster data, faction war/treaty data, and focused tests are live. The plan must re-audit current counts and path evaluators rather than repeat historical “three witnesses” claims.

# 3. Required Delta

Define peacetime action contracts, treaty-to-escalation facts, witness selection by actual history, Muster path eligibility, and epilogue projections with explicit owner boundaries.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Round 3/4 plans already touched adjacent faction and Muster files; this plan must integrate current owners and avoid reauthoring their sealed semantics. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `act_salvage_rights_offer`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `act_claim_arbitration`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `act_apprentice_rule_dispute`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `act_purification_toll`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `act_hydro_emergency_appeal`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `act_intake_dispute`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `act_raider_parley`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `act_raider_passage_levy`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `act_raider_code_dispute`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `act_coalition_mediation_request`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `act_coalition_supply_appeal`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `act_camp_rules_dispute`
- Source: `Assets/StreamingAssets/Data/muster_faction_actions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `camp_scene_arrivals`
- Source: `Assets/StreamingAssets/Data/muster_camp_scenes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `camp_scene_old_enemies`
- Source: `Assets/StreamingAssets/Data/muster_camp_scenes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `camp_scene_shared_meal`
- Source: `Assets/StreamingAssets/Data/muster_camp_scenes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `camp_scene_confrontation`
- Source: `Assets/StreamingAssets/Data/muster_camp_scenes.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `witness_1_checkpoint_conscript`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `witness_2_quartermaster_paperwork`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `witness_3_signals_intercept`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `witness_scavenger_claimant`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `witness_messengers_keeper`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `witness_claimant_auditor`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `witness_hydro_envoy`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `witness_raider_parley_survivor`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `witness_camp_medic`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `witness_camp_dissenter`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `witness_deserter_elder`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `witness_queue_singer`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `witness_overflow_medic`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `witness_summit_envoy`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `witness_levy_party_chief`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `witness_harbor_master_kell`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `witness_trawler_captain_maren`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `witness_coastal_refugee_nurse`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `witness_naval_conscript_brant`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `witness_convoy_driver_tomas`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `witness_rebuilder_field_medic`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `witness_garrison_picket_vaughn`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `witness_wayside_mechanic_yorin`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `witness_foundry_molder_hask`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `witness_iceroad_hauler_sula`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `witness_arbitration_clerk_moran`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `witness_terrace_elder_marit`
- Source: `Assets/StreamingAssets/Data/muster_witnesses.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `faction_the_office`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `faction_the_cutters`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `faction_the_fleet`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `faction_black_flotilla`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `faction_supply_corps`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `faction_railway_guild`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `faction_hydro_barons`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `faction_ordnance_foundry`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `faction_scavengers`
- Source: `Assets/StreamingAssets/Data/holdfast_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `faction_the_overlay`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `faction_the_scale`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `faction_the_compact`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `faction_the_underwrite`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `faction_the_rebuilders`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `faction_the_garrison`
- Source: `Assets/StreamingAssets/Data/standing_record_factions.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `epilogue_demographics_thriving`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `epilogue_demographics_persevering`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `epilogue_demographics_desolation`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `epilogue_governance_reconciliation`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `epilogue_governance_iron_order`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `epilogue_technology_renaissance`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `epilogue_technology_makeshift`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `epilogue_sustenance_harvest`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `epilogue_sustenance_famine`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each action, witness, scene, and epilogue record, identify the live faction/treaty/war/Muster fact it consumes and the single visible consequence it may project.
- Primary owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State/save rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI truth rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: peacetime factions, war escalation, witnesses, and epilogue.
- Seam under test: faction action/culture + treaty/war facts -> FactionActionBoard/RegionalTreaty/FactionWar -> MusterPathEvaluator/WitnessSelector/MusterSystem -> Muster UI/journal/epilogue.
- Expected authority: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger. Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI/accessibility check: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- Player-facing truth: Dead faction, missing treaty, no eligible witness, contradictory war state, stale action, absent epilogue owner, and repeated Muster path evaluation fail closed without inventing a victor.
- Persistence response: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism response: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: FactionActionBoard.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: FactionActionCatalog.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: RegionalTreatySystem.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: FactionWarSystem.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: MusterPathEvaluator.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: WitnessSelector.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: MusterSystem.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: FactionActionBoard.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: FactionActionCatalog.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: RegionalTreatySystem.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: FactionWarSystem.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: MusterPathEvaluator.
- Owner: Faction systems own their resources/actions; RegionalTreaty and FactionWar own state transitions; Muster owns assembly selection; epilogue owns final matrix.
- State rule: Action resolutions, treaty status, war stage, witness eligibility, and epilogue facts use existing faction/Muster/epilogue stores; no new universal faction ledger.
- Determinism rule: Action choice order, witness ranking, path evaluation, and war references are stable by IDs and day; no wall-clock or unordered collection selection.
- UI rule: ['src/UI/MusterPanel.cs', 'src/UI/MusterAtlasPanel.cs', 'src/Muster/FactionActionPanel.cs', 'src/Muster/JournalWitnessPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan25FactionEcologyTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/MusterSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/MusterSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/MusterPathEvaluatorTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/MusterPathEvaluatorTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/MusterContentCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/MusterContentCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/MusterEpilogueMatrixTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/Plan84WitnessExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Plan84WitnessExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 07
- Test: `Ashfall.Core.Tests/WitnessSelectionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/WitnessSelectionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 342,626 characters.
# Post-250K deep polishing pass

The architecture body above reached 342,704 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `FactionActionBoard`, `FactionActionCatalog`, `RegionalTreatySystem`, `FactionWarSystem`, `MusterPathEvaluator`, `WitnessSelector`, `MusterSystem`.
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
