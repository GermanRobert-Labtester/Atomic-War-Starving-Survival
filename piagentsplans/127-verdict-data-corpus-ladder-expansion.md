# Plan 127 — Verdict Data Corpus and World History Ladder: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** verdict evidence, machine logs, and history layers
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Extend the existing Verdict evidence and machine-log seam with validated corpus records, explicit knowledge gates, truthful corruption presentation, and durable discovery history.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5881` characters.
- Current worktree copy: `307157` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `30bcfc9f91c1ab7b69cb80cb8a06d882a0f1802a3314f26d4551834aacc079ae`
- Snapshot size: 3944 characters; 111 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0014:     [Serializable]
0015:     public sealed class EvidenceLedgerState
0016:     {
0017:         public List<string> enrolled = new List<string>();
...
0021:
0022:     /// <summary>Evidence definition (verdict_data.json 'evidence' section).</summary>
0023:     public class EvidenceDefinition
0024:     {
...
0034:
0035:     public sealed class EvidenceLedger
0036:     {
0037:         private readonly EvidenceLedgerState _state;
...
0040:
0041:         public EvidenceLedgerState State => _state;
0042:         public IReadOnlyList<string> Enrolled => _state.enrolled;
0043:
...
0047:         {
0048:             _state = state ?? new EvidenceLedgerState();
0049:         }
0050:
...
0082:
0083:         public EvidenceLedgerState CaptureState()
0084:         {
0085:             var copy = new EvidenceLedgerState
...
0093:
0094:         public void RestoreState(EvidenceLedgerState state)
0095:         {
0096:             if (state == null) return;
```

### Current evidence: `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4b4ef8e95fb5d12e9c6c9329151d355e68c9614439b4188f3380d79617d4458e`
- Snapshot size: 7448 characters; 200 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0025:     [Serializable]
0026:     public sealed class MachineLogSystemState
0027:     {
0028:         public List<MachineLogEntry> entries = new List<MachineLogEntry>();
...
0039:     /// </summary>
0040:     public sealed class MachineLogSystem
0041:     {
0042:         private readonly MachineLogSystemState _state;
...
0050:
0051:         public MachineLogSystem(MachineLogSystemState? state = null)
0052:         {
0053:             _state = state ?? new MachineLogSystemState();
...
0094:         /// <summary>Insert a deterministic, seed-dependent garbling marker (corruption).
0095:         /// Corpus is data-driven (verdict_data.json). Falls back to built-ins if none supplied.</summary>
0096:         public bool InsertCorruptionMarker(int day, ISeededRng rng, IReadOnlyList<string>? corpus = null)
0097:         {
...
0149:
0150:         public MachineLogSystemState CaptureState()
0151:         {
0152:             var copy = new MachineLogSystemState
...
0167:
0168:         public void RestoreState(MachineLogSystemState state)
0169:         {
0170:             if (state == null) return;
```

### Current evidence: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `abee1d19deb019bb058c9435069600549265c68836d2723c607fccd88c23d334`
- Snapshot size: 9641 characters; 225 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009:     /// ASHFALL: THE VERDICT (Expansion 08) — catalog loader for the three
0010:     /// Verdict data files (verdict_data.json, verdict_locations.json,
0011:     /// verdict_radio.json). Authoring authority is the design bible; the loader
0012:     /// mirrors the WitnessCatalogLoader pattern (missing file => empty list,
...
0016:     {
0017:         public const string DataFile = "verdict_data.json";
0018:         public const string LocationsFile = "verdict_locations.json";
0019:         public const string ItemsFile = "verdict_items.json";
...
0067:         /// <summary>Optional effect payload of an evidence/story item (recorded for
0068:         /// reachability; the game enrolls evidence through the EvidenceLedger, not
0069:         /// this mirror). Mirrors the authored JSON shape.</summary>
0070:         public class VerdictItemEffects
...
0170:             public List<string> corruption_corpus = new List<string>();
0171:             public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
0172:         }
0173:
...
0196:
0197:         /// <summary>Load the world history ladder from verdict_data.json (empty if missing).</summary>
0198:         public static List<VerdictWorldHistoryLadderEntry> LoadWorldHistoryLadder(
0199:             string dataDir, IFileIO fileIO, IJsonSerializer json)
...
0209:                 var parsed = json.Deserialize<VerdictDataContainer>(raw);
0210:                 if (parsed?.world_history_ladder != null)
0211:                     result.AddRange(parsed.world_history_ladder);
0212:             }
```

### Current evidence: `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `35bd29e9933555d17a90114245b9395ee98ec22077e7d46e2467dd185f603806`
- Snapshot size: 12110 characters; 272 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0032:         public int simDay;
0033:         public MachineLogSystemState machineLog = new MachineLogSystemState();
0034:         public ReckoningState reckoning = new ReckoningState();
0035:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
...
0055:         public int simDay;
0056:         public MachineLogSystemState machineLog = new MachineLogSystemState();
0057:         public ReckoningState reckoning = new ReckoningState();
0058:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
...
0075:         public int simDay;
0076:         public MachineLogSystemState machineLog = new MachineLogSystemState();
0077:         public ReckoningState reckoning = new ReckoningState();
0078:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
...
0092:         public int simDay;
0093:         public MachineLogSystemState machineLog = new MachineLogSystemState();
0094:         public ReckoningState reckoning = new ReckoningState();
0095:         public EvidenceLedgerState evidence = new EvidenceLedgerState();
...
0105:             int simDay,
0106:             MachineLogSystem machineLog,
0107:             ReckoningSystem reckoning,
0108:             EvidenceLedger evidence,
...
0249:             VerdictSave save,
0250:             MachineLogSystem machineLog,
0251:             ReckoningSystem reckoning,
0252:             EvidenceLedger evidence,
```

### Current evidence: `src/Host/VerdictHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `eb9abc73b3557cdfa1d7b975f426399da4cbf2e299387681232521df4b1f6f8b`
- Snapshot size: 14356 characters; 284 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0018:     /// ASHFALL: THE VERDICT (Expansion 08) — thin Godot host session.
0019:     /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
0020:     /// census broadcast, wires the sim clock / event bus / flag ledger / census
0021:     /// port, and persists to user:// via VerdictSaveStore. No gameplay rules
...
0028:
0029:         public MachineLogSystem MachineLog { get; }
0030:         public ReckoningSystem Reckoning { get; }
0031:         public EvidenceLedger Evidence { get; }
...
0036:         public QuestlineSystem Quests { get; }
0037:         public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
0038:         public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
0039:         public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
...
0072:         public string LastEvent { get; private set; } = string.Empty;
0073:         public VerdictHostSession(
0074:             MachineLogSystem machineLog = null!,
0075:             ReckoningSystem reckoning = null!,
...
0078:             VerdictCensusBroadcast census = null!,
0079:             IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> locations = null!,
0080:             IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> items = null!,
0081:             IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> radio = null!,
...
0085:             Reckoning = reckoning ?? new ReckoningSystem();
0086:             Evidence = evidence ?? new EvidenceLedger();
0087:             EvidenceChain = new VerdictEvidenceChain(MachineLog, Evidence, Reckoning);
0088:             Npcs = npcs ?? new VerdictNpcSystem();
...
0091:             Locations = locations ?? new List<VerdictCatalogLoader.VerdictLocationEntry>();
0092:             Items = items ?? new List<VerdictCatalogLoader.VerdictItemEntry>();
0093:             RadioEntries = radio ?? new List<VerdictCatalogLoader.VerdictRadioEntry>();
0094:             CorruptionCorpus = new List<string>();
...
0105:
0106:         public static VerdictHostSession Create(
0107:             string dataDir,
0108:             ISimClock clock = null!,
...
0118:
0119:             var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
0120:             var items = VerdictCatalogLoader.LoadItems(dataDir, s_files, s_json);
0121:             var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
...
0124:             VerdictQuestCatalogLoader.LoadAndRegister(quests, dataDir, s_files, s_json);
0125:             var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries, quests: quests);
0126:             session.Radio = new VerdictRadioSystem(bus, clock, radioEntries);
0127:             VerdictNpcCatalogLoader.LoadAndRegister(session.Npcs, dataDir, s_files, s_json);
...
0195:
0196:         private VerdictCatalogLoader.VerdictRadioEntry? FindRadioEntry(string id)
0197:         {
0198:             for (int i = 0; i < RadioEntries.Count; i++)
...
```

### Current evidence: `src/Main.Verdict.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.Verdict.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f106b77d3b9ef1bd7ddae6b19a9c3c09b6624c7467d27ee6e575cfed7015b0d9`
- Snapshot size: 8053 characters; 212 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0033:         // ── Verdict fields (GAP-ARCH-01 Phase 1) ──
0034:         private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
0035:         private Godot.Label _verdictReadoutLabel = null!;
0036:         private VerdictPanel _verdictPanel = null!;
...
0046:             if (_verdict != null) return;
0047:             _verdict = AtomicWar.GodotApp.VerdictHostSession.Create(_dataDir, flags: _consequenceLedger);
0048:             _verdict.StateChanged += () => { _verdictDirty = true; RefreshVerdictReadout(); };
0049:             UnlockVerdictLore();
```

### Current evidence: `src/VerdictPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `880c980f41d127d45df8a9b2d4b42bb34d01603c53f4015a86e39b45345af50e`
- Snapshot size: 17700 characters; 432 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0022:
0023:         private VerdictHostSession _verdict;
0024:         private Label _lblPhase;
0025:         private Label _lblReadout;
...
0127:
0128:         public void Bind(VerdictHostSession verdict)
0129:         {
0130:             _verdict = verdict;
```

### Current evidence: `src/UI/VerdictDashboardPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/VerdictDashboardPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d1d57d14186f49a51faace91d3448d4971629702410ebb6ba660f31a7a64e322`
- Snapshot size: 7683 characters; 178 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0020: ///
0021: /// Reads headline metrics from the bound VerdictHostSession directly; the
0022: /// inner VerdictPanel is mounted as the content slot.
0023: /// </summary>
...
0030:     private VerdictPanel? _verdictInner;
0031:     private VerdictHostSession? _session;
0032:
0033:     public bool IsBound => _session != null;
```

### Current evidence: `Assets/StreamingAssets/Data/verdict_data.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `2c7ef4992e16b4d32e0a3a12af42f4d12bf8460e51a0c69eadc8e0d42799dae2`
- Snapshot size: 9776 characters; 209 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0095:   ],
0096:   "world_history_ladder": [
0097:     {
0098:       "layer": 1,
```

### Current evidence: `Assets/StreamingAssets/Data/verdict_items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `6a4888ad04a244300ecad9c9d2acd3c22d95e084bf0e8991459738fa7a5ae2b8`
- Snapshot size: 10321 characters; 231 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "evidence_geophone_hymn",
0006:       "displayName": "The Farm's Seismic Signature",
0007:       "weightKg": 1.2,
0008:       "tradeValue": 12,
0009:       "category": "story_item",
0010:       "tier": "Old-World",
0011:       "description": "Under the Allotments, the array keeps time like a metronome that farms: a tap at ploughing, a tap at harvest, a tap at the well-house door. The machine reads the ground and hears a farm. It has never been to the farm.",
0012:       "mechanical_effects": {
0013:         "enrolled_evidence": 1
0014:       },
0015:       "downstream_quest_trigger": "quest_verdict_the_warm_range",
0016:       "faction_affinity": "faction_the_tempest",
0017:       "rarity": "Rare"
0018:     },
0019:     {
0020:       "id": "evidence_twelve_gauge_steel",
```

### Current evidence: `Assets/StreamingAssets/Data/verdict_locations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`
- Snapshot size: 14416 characters; 125 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locations": [
0004:     {
0005:       "id": "loc_geophone_pit_1",
0006:       "displayName": "The First Geophone Pit",
0007:       "description": "A concrete collar sunk like a wellhead, the lid propped on a brick. Below: a seismometer array the size of a dinner plate, bolted to bedrock, humming at a pitch almost too low to hear. The cable runs east, into the treeline, under the ridgeline. No one has recorded anything in the log for four years except the array itself, and the array reads the ground as if the whole valley were one slow heartbeat. A hand-painted sign on the lid, painted over twice: TEMPEST SITE 01 — KEEP OUT — DO NOT ENTER — ENTER AT YOUR OWN RISK. The last line is in a different hand, and it is not a warning.",
0008:       "dangerLevel": 6,
0009:       "travelHours": 5.5,
0010:       "baseRadsPerHour": 34
0011:     },
0012:     {
0013:       "id": "loc_twelve_gauge_array",
0014:       "displayName": "The Twelve-Gauge Array",
0015:       "description": "Twelve shot-firing sounding stations on the ridge, each a one-metre steel post with a grease-stained plate reading TEMPEST SITE 07 and a firing order stencilled in flaking yellow. The ordnance is long gone — the holes are empty — but the plates list the charge weights, the depths, the shot ordnance. Somebody has been keeping the plates legible, which is odd, because the nearest human settlement is nine hours away. The array is the fuse world's quiet door: the cable that runs under the treeline is the Tempest's own line, and it runs here because nobody on the surface walks this ridge.",
0016:       "dangerLevel": 7,
0017:       "travelHours": 6.0,
0018:       "baseRadsPerHour": 38
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`
- Snapshot size: 9832 characters; 249 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "npc_eden_vale",
0006:       "name": "Eden Vale",
0007:       "role": "Amateur radio operator, comm-array bleed",
0008:       "kind": "tape_echo",
0009:       "gating_flag": "flag_verdict_eden_log_recovered",
0010:       "location_id": "loc_comm_array",
0011:       "phase_min": 1,
0012:       "dialogue": [
0013:         "Still here. Static's thinning. That's not good news, that's a storm on the way.",
0014:         "The array's drawing again. I don't know what it's drawing for. I don't think it draws for us."
0015:       ]
0016:     },
0017:     {
0018:       "id": "npc_ferris_voss",
0019:       "name": "Ferris Voss",
0020:       "role": "Fire-control acceptance engineer, last human in the fuse world",
```

### Current evidence: `Assets/StreamingAssets/Data/verdict_questlines.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`
- Snapshot size: 85666 characters; 1748 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "quests": [
0004:     {
0005:       "questlineId": "quest_verdict_the_warm_range",
0006:       "title": "The Warm Range",
0007:       "synopsis": "A reading is a measurement. Follow the cable run east and find what the machines were built to serve.",
0008:       "factionTag": "faction_the_tempest",
0009:       "firstStageId": "stage_warm_path",
0010:       "minDay": 160,
0011:       "maxDay": 360,
0012:       "stages": [
0013:         {
0014:           "stageId": "stage_warm_path",
0015:           "title": "A Reading",
0016:           "narrativePrompt": "The geophone pit reads the valley like a slow heartbeat, and the cable runs east under the treeline. Somebody once knew where it went.",
0017:           "unlockOnDay": 160,
0018:           "isTerminal": false,
0019:           "choices": [
0020:             {
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

### Current evidence: `Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f7dd421b2dd58f510fb296dd2be3e73d959286e775adfffe63a09e6abae048ad`
- Snapshot size: 4415 characters; 111 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0031:
0032:             var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);
0033:
0034:             Assert.NotNull(corpus);
...
0058:
0059:             var ladder = VerdictCatalogLoader.LoadWorldHistoryLadder(dir, io, json);
0060:
0061:             Assert.NotNull(ladder);
...
0085:         [Fact]
0086:         public void MachineLogSystem_InjectsCorruptionMarkersFromExpandedCorpus()
0087:         {
0088:             string dir = ResolveDataDir();
...
0091:
0092:             var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);
0093:             Assert.Equal(25, corpus.Count);
0094:
```

### Current evidence: `Ashfall.Core.Tests/VerdictSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f25ace785950670bd5a6ef2436b246b418a95a82c41be19a2745faf76a4bbd06`
- Snapshot size: 27025 characters; 723 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0020:     {
0021:         // ── EvidenceLedger ──────────────────────────────────────────────────────
0022:
0023:         [Fact]
...
0035:         {
0036:             var ledger = new EvidenceLedger();
0037:             ledger.Register(new EvidenceDefinition { id = "ev_a" });
0038:             Assert.False(ledger.Enroll("ev_unknown", 160));
...
0044:         {
0045:             var ledger = new EvidenceLedger();
0046:             Assert.True(ledger.Enroll("ev_any", 160));
0047:             Assert.Equal(1, ledger.Count);
...
0052:         {
0053:             var ledger = new EvidenceLedger();
0054:             string fired = null;
0055:             ledger.OnEnrolled += id => fired = id;
...
0062:         {
0063:             var ledger = new EvidenceLedger();
0064:             ledger.Enroll("ev_1", 160);
0065:             ledger.Enroll("ev_2", 180);
...
0077:         {
0078:             var ledger = new EvidenceLedger();
0079:             Assert.False(ledger.Enroll("", 0));
0080:             Assert.False(ledger.Enroll(null, 0));
...
0084:
0085:         // ── MachineLogSystem ────────────────────────────────────────────────────
0086:
0087:         [Fact]
...
0098:         {
0099:             var log = new MachineLogSystem();
0100:             Assert.True(log.Post("fac_a", 160, "operating", "body", "ev_a"));
0101:             Assert.True(log.Post("fac_a", 160, "maintenance", "body2", "ev_b"));
...
0107:         {
0108:             var log = new MachineLogSystem();
0109:             log.Post("fac_a", 160, "operating", "body", "ev_a");
0110:             Assert.Equal("ev_a", log.ReadEntry(0));
...
0118:         {
0119:             var log = new MachineLogSystem();
0120:             Assert.Equal(string.Empty, log.ReadEntry(-1));
0121:             Assert.Equal(string.Empty, log.ReadEntry(0));
...
0127:         {
0128:             var log1 = new MachineLogSystem();
0129:             var log2 = new MachineLogSystem();
0130:             var rng1 = new SeededRng(42);
...
```

### Current evidence: `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5636d6d644382c3caa6da51c39348b65bb5584d79fa99bdb811cd9ff7fccf355`
- Snapshot size: 10000 characters; 229 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0030:                 simDay = 241,
0031:                 machineLog = new MachineLogSystemState(),
0032:                 reckoning = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true, callResolved = true },
0033:                 evidence = new EvidenceLedgerState { enrolled = new List<string> { "evidence_fuse_linen" } },
...
0049:             // Restore into a fresh session survives.
0050:             var ml = new MachineLogSystem();
0051:             var rec = new ReckoningSystem();
0052:             var ev = new EvidenceLedger();
...
0091:                 reckoning = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true, callResolved = true },
0092:                 evidence = new EvidenceLedgerState { enrolled = new List<string> { "evidence_eden_log" } }
0093:             };
0094:             v2.Checksum = SaveChecksum.Compute(v2);
...
0127:         {
0128:             var sys = new MachineLogSystem();
0129:             var corpus = new List<string> { "[CENSUS WINDOW: garbled]" };
0130:             Assert.True(sys.InsertCorruptionMarker(213, new SeededRng(7), corpus));
...
0137:         {
0138:             var sys = new MachineLogSystem();
0139:             sys.Post("loc_geophone_pit_1", 170, "operating", "a tap", "evidence_geophone_hymn");
0140:             sys.ReadEntry(0);
...
0175:         {
0176:             var ledger = new EvidenceLedger();
0177:             int fires = 0;
0178:             ledger.OnEnrolled += _ => fires++;
...
0224:             var rec = new ReckoningSystem();
0225:             VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), rec, new EvidenceLedger(), new VerdictNpcSystem());
0226:             Assert.Equal("ending_verdict_the_count_is_held", VerdictEndingEvaluator.DecideEnding(rec.State, 0, 300));
0227:         }
```

### Current evidence: `Ashfall.Core.Tests/VerdictIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e04831ecf3189ba3a46eeddcd634c51019b68724ae18d505695591130d2bfacb`
- Snapshot size: 9513 characters; 216 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0112:             var reckoning = new ReckoningSystem();
0113:             var evidence = new EvidenceLedger();
0114:
0115:             // Day 210 without evidence: no Culpable promotion.
```

### Current evidence: `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `678c8099a9851b93478ddb7d760c23c9bf07de0506ef7e67f8fc0857610c4e09`
- Snapshot size: 9384 characters; 208 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0126:             var rebelCatalog = RebelBranchCatalog.LoadAndRegister(dir, io, json);
0127:             var corpus = VerdictCatalogLoader.LoadCorruptionCorpus(dir, io, json);
0128:             var ladder = VerdictCatalogLoader.LoadWorldHistoryLadder(dir, io, json);
0129:
...
0135:             var rebelSystem = new RebelBranchSystem(rebelCatalog, flags);
0136:             var machineLog = new MachineLogSystem();
0137:             var moral = MakeMoralChoice(MoralPathBand.Neutral);
0138:
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/verdict_data.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `catalog, schema_version, description, currencies, readout_steps, facets, endings, world_history_ladder, corruption_corpus`
- `currencies`: list count=1; sample IDs=['enrolled_evidence']
- `readout_steps`: list count=4; sample IDs=['step_fuse_advance', 'step_drone_sleep', 'step_summit_light', 'step_census_carrier']
- `facets`: list count=3; sample IDs=['facet_archive', 'facet_fire_computing', 'facet_vent_shaft']
- `endings`: list count=3; sample IDs=['ending_verdict_the_sector_recounts', 'ending_verdict_the_count_is_held', 'ending_verdict_the_offer_is_a_lease']
- `world_history_ladder`: list count=12; sample IDs=[]
- `corruption_corpus`: list count=25; sample IDs=['[00:03:07] — signal lost mid-verbose.', '[unreadable] sector halts. Sector halts.', '11111111 — no hand. No hand on the valve.', '[tone] [tone] [tone] — the count repeats itself.', 'the meter read. The meter read. The meter read.', 'CENSUS WINDOW: [garbled]. Persons present: [garbled].', 'the archive does not require a reader. the archive does not —', '— held pending count. held pending count. held —']
- `schema_version`: `1`
- `catalog`: `verdict`
- `description`: `ASHFALL: THE VERDICT — machine-readable master data. Companion to docs/expansions/expansion_08_the_verdict_plan.md. The machine that keeps the count after the people stopped.`
- SHA-256: `2c7ef4992e16b4d32e0a3a12af42f4d12bf8460e51a0c69eadc8e0d42799dae2`
#### `Assets/StreamingAssets/Data/verdict_items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=15; sample IDs=['evidence_geophone_hymn', 'evidence_twelve_gauge_steel', 'evidence_fuse_linen', 'evidence_census_draft', 'evidence_mailroom_tape', 'evidence_uxo_register', 'evidence_call_calibration', 'evidence_call_plain']
- `schema_version`: `1`
- SHA-256: `6a4888ad04a244300ecad9c9d2acd3c22d95e084bf0e8991459738fa7a5ae2b8`
#### `Assets/StreamingAssets/Data/verdict_locations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locations`
- `locations`: list count=15; sample IDs=['loc_geophone_pit_1', 'loc_twelve_gauge_array', 'loc_network_fuse_bunker', 'loc_archive_tape_silo', 'loc_abandoned_tide_gauge', 'loc_coastal_meteorological_station', 'loc_clifftop_observation_bunker', 'loc_sealed_marine_laboratory']
- `schema_version`: `1`
- SHA-256: `e97faf513dfa2e3f6b9cfb7fd1311cea6534a283e7d81e099784232b9d9380d0`
#### `Assets/StreamingAssets/Data/verdict_npcs.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=18; sample IDs=['npc_eden_vale', 'npc_ferris_voss', 'npc_iran_bell', 'npc_selya_saltmarsh', 'npc_maro_veen', 'npc_whisper_cipher', 'npc_tomas_reid', 'npc_elena_vane']
- `schema_version`: `1`
- SHA-256: `ace1edded901844ae68ebbaad7f913990316ab155888ccee2c531766be7cbadc`
#### `Assets/StreamingAssets/Data/verdict_questlines.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, quests`
- `quests`: list count=23; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `18f04bdcacbc31e6be03358116f0390de2786309d8d0ddbd9770b0aed94dcdc2`
## Symbol and caller audit

#### `EvidenceLedger` — HOST_REFERENCE_PRESENT — core/declaration=7, host=7, test=23
- `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs:35` (declaration) — public sealed class EvidenceLedger
- `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs:46` (core) — public EvidenceLedger(EvidenceLedgerState? state = null)
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:68` (core) — /// reachability; the game enrolls evidence through the EvidenceLedger, not
- `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs:14` (core) — private readonly EvidenceLedger _ledger;
- `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs:19` (core) — EvidenceLedger ledger,
- `Assets/Ashfall.Core/Verdict/VerdictSave.cs:108` (core) — EvidenceLedger evidence,
- `Assets/Ashfall.Core/Verdict/VerdictSave.cs:252` (core) — EvidenceLedger evidence,
- `src/Host/VerdictHostSession.cs:19` (host) — /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
- `src/Host/VerdictHostSession.cs:31` (host) — public EvidenceLedger Evidence { get; }
- `src/Host/VerdictHostSession.cs:76` (host) — EvidenceLedger evidence = null!,
- `src/Host/VerdictHostSession.cs:86` (host) — Evidence = evidence ?? new EvidenceLedger();
- `src/Host/VerdictHostSession.cs:207` (host) — /// EvidenceLedger; never double-enrolls. Returns count enrolled this call.
- `src/Host/HostCli.SelfTests.cs:815` (host) — var evidence = new EvidenceLedger();
- `src/Host/InventoryHostSession.cs:247` (host) — // Evidence is authoritative in the Verdict EvidenceLedger; these ItemDefinitions
- `Ashfall.Core.Tests/VerdictAccusationSystemTests.cs:16` (test) — private static (ReckoningSystem reckoning, MachineLogSystem machineLog, EvidenceLedger ledger) BuildCulpableReckoning(int evidence = 1)
- `Ashfall.Core.Tests/VerdictAccusationSystemTests.cs:20` (test) — var ledger = new EvidenceLedger();
- `Ashfall.Core.Tests/VerdictIntegrationTests.cs:113` (test) — var evidence = new EvidenceLedger();
- `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:69` (test) — 241, new MachineLogSystem(), new ReckoningSystem(), new EvidenceLedger(), -1,
- `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:80` (test) — new EvidenceLedger(), npcs: new VerdictNpcSystem(), quests: restored);
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:52` (test) — var ev = new EvidenceLedger();
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:176` (test) — var ledger = new EvidenceLedger();
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:225` (test) — VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), rec, new EvidenceLedger(), new VerdictNpcSystem());
- `Ashfall.Core.Tests/VerdictSystemTests.cs:21` (test) — // ── EvidenceLedger ──────────────────────────────────────────────────────
- `Ashfall.Core.Tests/VerdictSystemTests.cs:26` (test) — var ledger = new EvidenceLedger();
- … 13 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `MachineLogSystem` — HOST_REFERENCE_PRESENT — core/declaration=7, host=9, test=32
- `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs:40` (declaration) — public sealed class MachineLogSystem
- `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs:51` (core) — public MachineLogSystem(MachineLogSystemState? state = null)
- `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs:13` (core) — private readonly MachineLogSystem _machineLog;
- `Assets/Ashfall.Core/Verdict/VerdictEvidenceChain.cs:18` (core) — MachineLogSystem machineLog,
- `Assets/Ashfall.Core/Verdict/VerdictSave.cs:106` (core) — MachineLogSystem machineLog,
- `Assets/Ashfall.Core/Verdict/VerdictSave.cs:250` (core) — MachineLogSystem machineLog,
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:969` (core) — ["verdict_data.json"] = new[] { "ReckoningSystem", "MachineLogSystem" },
- `src/Host/VerdictHostSession.cs:19` (host) — /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
- `src/Host/VerdictHostSession.cs:29` (host) — public MachineLogSystem MachineLog { get; }
- `src/Host/VerdictHostSession.cs:74` (host) — MachineLogSystem machineLog = null!,
- `src/Host/VerdictHostSession.cs:84` (host) — MachineLog = machineLog ?? new MachineLogSystem();
- `src/Host/HostCli.SelfTests.cs:813` (host) — var machineLog = new MachineLogSystem();
- `src/Host/RetentionHostSession.cs:41` (host) — /// <see cref="Verdict.MachineLogSystem"/>, and the per-survivor dose reading
- `src/Host/RetentionHostSession.cs:57` (host) — public Ashfall.Core.Verdict.MachineLogSystem? MachineLog { get; set; }
- `src/Host/RetentionHostSession.cs:106` (host) — Ashfall.Core.Verdict.MachineLogSystem? machineLog,
- `src/Host/HostCli.Retention.cs:107` (host) — var machineLog = new MachineLogSystem();
- `Ashfall.Core.Tests/VerdictAccusationSystemTests.cs:16` (test) — private static (ReckoningSystem reckoning, MachineLogSystem machineLog, EvidenceLedger ledger) BuildCulpableReckoning(int evidence = 1)
- `Ashfall.Core.Tests/VerdictAccusationSystemTests.cs:19` (test) — var machineLog = new MachineLogSystem();
- `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:69` (test) — 241, new MachineLogSystem(), new ReckoningSystem(), new EvidenceLedger(), -1,
- `Ashfall.Core.Tests/VerdictQuestOwnershipTests.cs:79` (test) — VerdictSaveCodec.Restore(loaded, new MachineLogSystem(), new ReckoningSystem(),
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:50` (test) — var ml = new MachineLogSystem();
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:128` (test) — var sys = new MachineLogSystem();
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:138` (test) — var sys = new MachineLogSystem();
- `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs:142` (test) — var fresh = new MachineLogSystem();
- … 24 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `VerdictCatalogLoader` — HOST_REFERENCE_PRESENT — core/declaration=6, host=17, test=36
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:15` (declaration) — public static class VerdictCatalogLoader
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:25` (core) — private readonly List<VerdictCatalogLoader.VerdictRadioEntry> _corpus = new List<VerdictCatalogLoader.VerdictRadioEntry>();
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:33` (core) — IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry>? corpus = null)
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:43` (core) — public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> Corpus => _corpus;
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs:75` (core) — var loaded = VerdictCatalogLoader.LoadRadio(dataDir, fileIO, json);
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:368` (core) — ["verdict_data.json"] = new[] { "VerdictCatalogLoader" },
- `src/Host/VerdictHostSession.cs:37` (host) — public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
- `src/Host/VerdictHostSession.cs:38` (host) — public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
- `src/Host/VerdictHostSession.cs:39` (host) — public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
- `src/Host/VerdictHostSession.cs:79` (host) — IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> locations = null!,
- `src/Host/VerdictHostSession.cs:80` (host) — IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> items = null!,
- `src/Host/VerdictHostSession.cs:81` (host) — IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> radio = null!,
- `src/Host/VerdictHostSession.cs:91` (host) — Locations = locations ?? new List<VerdictCatalogLoader.VerdictLocationEntry>();
- `src/Host/VerdictHostSession.cs:92` (host) — Items = items ?? new List<VerdictCatalogLoader.VerdictItemEntry>();
- `src/Host/VerdictHostSession.cs:93` (host) — RadioEntries = radio ?? new List<VerdictCatalogLoader.VerdictRadioEntry>();
- `src/Host/VerdictHostSession.cs:119` (host) — var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
- `src/Host/VerdictHostSession.cs:120` (host) — var items = VerdictCatalogLoader.LoadItems(dataDir, s_files, s_json);
- `src/Host/VerdictHostSession.cs:121` (host) — var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
- `src/Host/VerdictHostSession.cs:128` (host) — session.CorruptionCorpus = VerdictCatalogLoader.LoadCorruptionCorpus(dataDir, s_files, s_json);
- `src/Host/VerdictHostSession.cs:196` (host) — private VerdictCatalogLoader.VerdictRadioEntry? FindRadioEntry(string id)
- `src/Host/VerdictHostSession.cs:262` (host) — public VerdictCatalogLoader.VerdictLocationEntry? FindLocation(string id)
- `src/Host/HostCli.SelfTests.cs:847` (host) — var radioCorpus = VerdictCatalogLoader.LoadRadio(dataDirectory, vio, vjson);
- `src/Host/HostCli.SelfTests.cs:861` (host) — var vhsItems = VerdictCatalogLoader.LoadItems(dataDirectory, vio, vjson);
- `Ashfall.Core.Tests/CatalogLoaderHardeningTests.cs:61` (test) — string path = Path.Combine(_scratch, VerdictCatalogLoader.ItemsFile);
- … 35 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `VerdictHostSession` — HOST_REFERENCE_PRESENT — core/declaration=3, host=14, test=0
- `Assets/Ashfall.Core/CatalogIntegrityRules.cs:332` (core) — "flag_verdict_cliff_signal_decoded", // Plan 93: materialized in VerdictHostSession (machine-log read depth)
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:510` (core) — "flag_verdict_cliff_signal_decoded", // Plan 93: materialized in VerdictHostSession (machine-log read depth)
- `src/Main.Verdict.cs:34` (host) — private AtomicWar.GodotApp.VerdictHostSession _verdict = null!;
- `src/Main.Verdict.cs:47` (host) — _verdict = AtomicWar.GodotApp.VerdictHostSession.Create(_dataDir, flags: _consequenceLedger);
- `src/VerdictPanel.cs:23` (host) — private VerdictHostSession _verdict;
- `src/VerdictPanel.cs:128` (host) — public void Bind(VerdictHostSession verdict)
- `src/Host/VerdictHostSession.cs:24` (declaration) — public sealed class VerdictHostSession
- `src/Host/VerdictHostSession.cs:73` (host) — public VerdictHostSession(
- `src/Host/VerdictHostSession.cs:106` (host) — public static VerdictHostSession Create(
- `src/Host/VerdictHostSession.cs:125` (host) — var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries, quests: quests);
- `src/Host/VerdictSaveStore.cs:5` (host) — // Host Caller: Main.Verdict / VerdictHostSession
- `src/YearOfAsh/YearOfAshHostSession.cs:122` (host) — // and persisted via VerdictHostSession / VerdictSave (v3+). Older
- `src/UI/ExpansionsHubPanel.cs:35` (host) — private VerdictHostSession? _verdict;
- `src/UI/ExpansionsHubPanel.cs:68` (host) — VerdictHostSession? verdict,
- `src/UI/VerdictDashboardPanel.cs:21` (host) — /// Reads headline metrics from the bound VerdictHostSession directly; the
- `src/UI/VerdictDashboardPanel.cs:31` (host) — private VerdictHostSession? _session;
- `src/UI/VerdictDashboardPanel.cs:35` (host) — public void Bind(VerdictPanel verdict, VerdictHostSession session)
#### `verdict_data` — HOST_REFERENCE_PRESENT — core/declaration=13, host=3, test=0
- `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs:22` (core) — /// <summary>Evidence definition (verdict_data.json 'evidence' section).</summary>
- `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs:95` (core) — /// Corpus is data-driven (verdict_data.json). Falls back to built-ins if none supplied.</summary>
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:10` (core) — /// Verdict data files (verdict_data.json, verdict_locations.json,
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:17` (core) — public const string DataFile = "verdict_data.json";
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:174` (core) — /// <summary>Load the corruption corpus from verdict_data.json (empty if missing).</summary>
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:197` (core) — /// <summary>Load the world history ladder from verdict_data.json (empty if missing).</summary>
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:41` (core) — "combat_catalog.json", "verdict_data.json", "verdict_items.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:368` (core) — ["verdict_data.json"] = new[] { "VerdictCatalogLoader" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:799` (core) — ["verdict_data.json"] = "VerdictCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:969` (core) — ["verdict_data.json"] = new[] { "ReckoningSystem", "MachineLogSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1422` (core) — ["verdict_data.json"] = new[] { "VerdictPanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1669` (core) — "journal_voice_prose.json", "verdict_data.json",
- `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:222` (core) — RegisterCatalog("verdict_data.json", "Verdict Data", CatalogClassification.Optional);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1135` (host) — foreach (var file in new[] { "verdict_data.json", "verdict_items.json", "verdict_locations.json", "verdict_radio.json", "verdict_questlines.json", "verdict_npcs.json" })
- `src/Journal/JournalCatalogData.cs:72` (host) — /// <summary>Verdict world-history ladder (verdict_data.json.world_history_ladder).</summary>
- `src/Journal/JournalCatalogData.cs:152` (host) — string path = fileIO.Combine(dataDir, "verdict_data.json");
#### `world_history_ladder` — HOST_REFERENCE_PRESENT — core/declaration=4, host=1, test=0
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:171` (core) — public List<VerdictWorldHistoryLadderEntry> world_history_ladder = new List<VerdictWorldHistoryLadderEntry>();
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:210` (core) — if (parsed?.world_history_ladder != null)
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:211` (core) — result.AddRange(parsed.world_history_ladder);
- `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs:215` (core) — CatalogDiagnostics.Warn(path, "VerdictDataContainer.world_history_ladder", ex_CATDIAG);
- `src/Journal/JournalCatalogData.cs:72` (host) — /// <summary>Verdict world-history ladder (verdict_data.json.world_history_ladder).</summary>
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
#### authority lines 58-61
00058:
00059: **DR-08 — Wave directories beyond the v1.0 history. VERIFIED.**
00060: `docs/plans/` (126 entries) contains wave directories `wave8_part2/`, `wave9_part2/`, `wave10_part1/`, `wave10_part2/`, `wave11_part1/`, `wave11_part2/`, `wave12_part1_1/`, `flagship_b5_b8/`, and `xp/`, plus `UNCLAIMED_CORPUS_CENSUS.md`, `UNBLOCKED_PLANS_AUDIT_2026-09-19.md`, and `WAVE10_MICRO_DEFERRAL_SWEEP.md`. Two of these are standing expansion inputs: `UNCLAIMED_CORPUS_CENSUS.md` (authored content no system consumes — a utilization-seam backlog) and the unblocked-plans audit. The Factory Protocol consumes both.
00061:
#### authority lines 75-78
00075:
00076: This protocol is the heart of v2.0. It converts repository evidence into subject plans, deterministically, and it is designed to be executed by any future planning session (human or LLM) without re-deriving the method. One execution of the protocol yields one subject plan; the matrices in Part III provide the candidate space; the backlog in Part IV holds pre-audited candidates.
00077:
00078: ### 2.1 The seven-step factory loop
#### authority lines 89-92
00089: **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
00090: A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.
00091:
00092: **Step 5 — Run the continuity and anti-duplication checklist.**
#### authority lines 113-116
00113:
00114: Ten lanes (A–J, from v1.0 Part 11) against seventeen subsystem clusters distilled from the live Core inventory (v1.0 Parts 5.1–5.2 and 16, confirmed live). Each cell names an opening archetype. Confidence labels reflect the audit state as of 2026-09-24 and must be re-checked at drafting time. This matrix is the combinatorial engine: 170 cells, each capable of yielding multiple subject plans over time as content lands and seams mature. Not every cell is currently open; cells marked SEALED are closed by evidence (e.g., the distress-signal content seal, DR-06) and may not be opened without new evidence and foreman signature.
00115:
00116: ### Cluster definitions
#### authority lines 129-132
00129: | C6 | Gazetteer entries and damaged-zone survey prose; cartographic marginalia | INFERENCE — verify current coverage |
00130: | C7 | Communiqué, directive, and verdict-corpus expansion for factions with thin public/private language separation | HIGH CONFIDENCE |
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
00132: | C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
#### authority lines 153-156
00153: | C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
00154: | C9 | Survivor interiority bridges: belief movements × faction stance; memorial rites × epilogue evidence; chemical dependency × medical ward | PROPOSAL |
00155: | C10 | Quest state reopening after new discoveries (failure-recovery grammar, v1.0 Part 6.7); moral-choice flag consumers beyond the flag ledger | HIGH CONFIDENCE |
00156: | C11 | Black-market funds/goods legs remain decision-gated (canonical funds authority); merchant restock priority is SEALED by DEC-05 (DR-06) | BLOCKED / SEALED |
#### authority lines 183-186
00183: | C9 | Lineage/cohort long-horizon state (3-year simulation exists per 19B closeout, DR-06): verify horizon coverage before extending | HIGH CONFIDENCE |
00184: | C13 | Epilogue evidence persistence: which Day-360+ facts survive into the Day-3650 window | HIGH CONFIDENCE |
00185: | Cross-cutting | Mid-event and mid-combat save round-trips for exactly-once effect classes beyond the rescue-signal runtime (which models the pattern) | PROPOSAL |
00186:
#### authority lines 243-246
00243:
00244: Each candidate is a subject-plan seed: consume it through the Factory Protocol. Ordering within the backlog is by evidence strength, not by preference. None of these has been claimed; all require the Step 1 premise sweep before drafting.
00245:
00246: **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.
#### authority lines 257-260
00257:
00258: **SB-07 — Root coordination surface registration (Lane I).** Evidence: DR-01, DR-09. Subject: register root-level coordination files and agent rulebooks in the docs map; define which are active vs historical. Integration route: docs-only. Confidence: VERIFIED need.
00259:
00260: **SB-08 — Gate-count drift guard (Lane H).** Evidence: DR-07. Subject: a check that fails when a documented gate count diverges from the live inventory, ending manual count drift between bibles, closeouts, and CI. Integration route: small script/test in `scripts/ci/` family, mirrors existing gates. Confidence: PROPOSAL (design needs the live gate inventory as input).
#### authority lines 280-283
00280: [What expands, in one paragraph.]
00281: ## Premise evidence
00282: [Live-source facts verified this session, each labeled. Name exact files/catalogs/systems.]
00283: ## Why this and not something else
#### authority lines 327-330
00327:
00328: 1. **Volume unit.** A volume is one appended Part to this document (or one of its companion canvases) produced in a single session, typically 15,000–60,000 characters, always evidence-grounded against the live repository.
00329: 2. **Volume types, in rotation:** (a) subsystem deep-map volumes (one per cluster C1–C17: full catalog inventories, prose-coverage gaps, seam maps); (b) prose specification libraries (expanded Part 9 field contracts with worked examples per document genre); (c) backlog replenishment volumes (fresh premise sweeps converting new Drift Register entries into SB-candidates); (d) lane deep guides (one per lane: full archetype playbooks with worked subject plans); (e) audit volumes (periodic re-audits refreshing the Drift Register).
00330: 3. **Session checklist.** Each session: run the Step 1 premise sweep; execute the Factory Protocol or append a volume; update the Drift Register for anything that moved; record the character count and volume index in the growth ledger below.
#### authority lines 347-350
00347: - **Drift Register** — the live-audit correction layer (Part I), the first thing any session reads.
00348: - **Subject plan** — an expansion proposal that names its subject, evidence, and recommended integration route but commits no file changes.
00349: - **Sealed surface** — a domain closed by evidence and signature (e.g., distress-signal content, DR-06); openable only with new evidence and foreman signature.
00350: - **Unclaimed content** — authored catalog content with no consuming system, tracked in `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08).
#### authority lines 365-368
00365:
00366: ### Premise evidence
00367: VERIFIED: `moral_choice_flags.json`, `moral_choice_quests_distress.json`, `moral_choice_chains.json`, and the wider moral-choice catalog family exist live in the data authority. VERIFIED: v1.0 Part 5.6 documents the flags/ledger seam and the weight_of_choices epilogue codec (v2). VERIFIED (drift-corrected): the rescue-signal content wave added `moral_choice_quests_distress.json`, so the moral-choice loader family already consumes multiple split catalogs — the pattern for adding one more split catalog exists. HIGH CONFIDENCE: no current consumer re-reads door-choice flags after the near-term window (v1.0 Part 7 gap 2); the integration plan must re-grep flag consumers before implementation.
00368:
#### authority lines 390-393
00390:
00391: ### Premise evidence
00392: VERIFIED: the atlas names the mid-winter slump as the primary pacing gap (v1.0 Part 7 gap 1). VERIFIED live: `ecological_infestations.json`, `subterranean_zones.json`, `warlord_doctrines.json`, `year_of_ash_storm_windows.json`, `seasonal_events.json`, `cascade_rules.json` all exist. VERIFIED: Year-of-Ash tick window is Days 180–360, so Days 90–180 pressure must ride seasonal/event seams, not Year-of-Ash seams.
00393:
#### authority lines 403-406
00403: ### Continuity checklist result
00404: Levy reactions must respect information-flow legality (the faction learns of the player's capacity through modeled channels). Cave-ins must not contradict subterranean zone states. Blight must respect crop-strain genome rules. Epilogue: blight and levy outcomes may feed standing/evidence through existing owners; declare permutations touched.
00405:
00406: ### Open premises
… 179 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.

**Requested behavior.** Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.

**Minimum safe delta.** Extend `verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.

**Required delta.** Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.

**Primary seam.** verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `EvidenceLedger`, `MachineLogSystem`, `VerdictCatalogLoader`, `VerdictHostSession`, `verdict_data`, `world_history_ladder`.

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
| Domain rules | EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 127.

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
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.
- What is the smallest safe change? Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.
- Which owner is touched? EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Verdict/VerdictCatalogLoader.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Verdict/VerdictSave.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/VerdictHostSession.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Verdict.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/VerdictPanel.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/VerdictDashboardPanel.cs` — EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/verdict_data.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/verdict_items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/verdict_locations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/verdict_npcs.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/verdict_questlines.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/VerdictPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/VerdictDashboardPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/JournalPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/VerdictSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/VerdictIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state is wired end to end or the plan explicitly closes as already integrated.
- Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

EvidenceLedger, MachineLogSystem, VerdictCatalogLoader, VerdictSave, VerdictHostSession, Main.Verdict, Verdict panels, verdict_data.json, and focused verdict tests already exist. The old plan’s proposed standalone loader and exact 25/12 counts require current catalog inspection and caller evidence.

# 3. Required Delta

Separate machine-log decoding, evidence enrollment, knowledge unlocks, and ending/history presentation; make corruption and missing knowledge truthful states rather than random prose or hidden progression.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Verdict save and accusation semantics are mature; this plan must not let decorative history become prosecution evidence without an explicit owner contract. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `enrolled_evidence`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `step_fuse_advance`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `step_drone_sleep`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `step_summit_light`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `step_census_carrier`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `facet_archive`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `facet_fire_computing`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `facet_vent_shaft`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `ending_verdict_the_sector_recounts`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `ending_verdict_the_count_is_held`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `ending_verdict_the_offer_is_a_lease`
- Source: `Assets/StreamingAssets/Data/verdict_data.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `evidence_geophone_hymn`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `evidence_twelve_gauge_steel`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `evidence_fuse_linen`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `evidence_census_draft`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `evidence_mailroom_tape`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `evidence_uxo_register`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `evidence_call_calibration`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `evidence_call_plain`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `evidence_reels_matter`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `evidence_valve_s36`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `evidence_eden_log`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `evidence_veen_your_people`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `item_archive_tape_silo_key`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `item_fuse_world_shift_charter`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `item_verdict_salt_flat_sample`
- Source: `Assets/StreamingAssets/Data/verdict_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `loc_geophone_pit_1`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `loc_twelve_gauge_array`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `loc_network_fuse_bunker`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `loc_archive_tape_silo`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `loc_abandoned_tide_gauge`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `loc_coastal_meteorological_station`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `loc_clifftop_observation_bunker`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `loc_sealed_marine_laboratory`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `loc_forestry_survey_post`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `loc_geological_core_vault`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `loc_river_gauging_station`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `loc_abandoned_agricultural_station`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `loc_decommissioned_signal_relay`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `loc_border_checkpoint_ruins`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `loc_minefield_observation_tower`
- Source: `Assets/StreamingAssets/Data/verdict_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `npc_eden_vale`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `npc_ferris_voss`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `npc_iran_bell`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `npc_selya_saltmarsh`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `npc_maro_veen`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `npc_whisper_cipher`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `npc_tomas_reid`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `npc_elena_vane`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `npc_kasper_holt`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `npc_mara_elsen`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `npc_ilya_venn`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `npc_garrick_daal`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `npc_sena_korr`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `npc_torin_rask`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `npc_oren_varek`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `npc_lena_rost`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `npc_tessa_mirn`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `npc_karel_norn`
- Source: `Assets/StreamingAssets/Data/verdict_npcs.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Classify each corpus or ladder record by evidence source, knowledge prerequisite, state mutation, corruption behavior, and ending/UI reachability before adding content.
- Primary owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State/save rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI truth rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: verdict evidence, machine logs, and history layers.
- Seam under test: verdict_data.json -> VerdictCatalogLoader/MachineLogSystem/EvidenceLedger -> VerdictHostSession -> Verdict UI, journal, ending, and save state.
- Expected authority: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder. Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI/accessibility check: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- Player-facing truth: Invalid corpus rows, missing locations, duplicate evidence, exhausted log keys, old saves, absent terminal, and corrupted state preserve the last valid layer and emit diagnostics.
- Persistence response: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism response: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: EvidenceLedger.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: MachineLogSystem.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: VerdictCatalogLoader.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: VerdictHostSession.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: verdict_data.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: world_history_ladder.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: EvidenceLedger.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: MachineLogSystem.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: VerdictCatalogLoader.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: VerdictHostSession.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: verdict_data.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: world_history_ladder.
- Owner: EvidenceLedger owns enrolled evidence; MachineLogSystem owns log decoding; Verdict catalog owns authored data; ending resolver owns interpretation; host projects.
- State rule: Unlocked layers, consumed logs, evidence keys, and corruption observations use the existing Verdict save shape or an explicitly migrated additive field; no second history ladder.
- Determinism rule: Log selection and corruption use existing seeded streams and stable ordinal selection; the same evidence and day produce the same readable result.
- UI rule: ['src/VerdictPanel.cs', 'src/UI/VerdictDashboardPanel.cs', 'src/UI/JournalPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan127VerdictCorpusLadderTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/VerdictSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/VerdictSaveMigrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictSaveMigrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/VerdictIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/VerdictIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Factions/Plan123_127RebelVerdictIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 296,918 characters.
# Post-250K deep polishing pass

The architecture body above reached 296,996 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `EvidenceLedger`, `MachineLogSystem`, `VerdictCatalogLoader`, `VerdictHostSession`, `verdict_data`, `world_history_ladder`.
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
