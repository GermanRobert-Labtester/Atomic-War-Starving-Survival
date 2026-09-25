# Plan 21 — Phantom Memory and Heirloom World Layer: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** memory, inheritance, confession, and provenance
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Build a provenance-first world layer over existing phantom, heirloom, memorial, confession, and journal owners, with no supernatural effects or duplicate inheritance ledger.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5959` characters.
- Current worktree copy: `489575` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/PhantomMemoryEngine.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a7c8cd4b1013c92912dc812d984500d87cb579c4bdd2e66c64ab48c5aa018ca6`
- Snapshot size: 20849 characters; 454 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0043:     [Serializable]
0044:     public class PhantomMemoryEngineState
0045:     {
0046:         public string systemId = PhantomMemoryEngine.SystemId;
...
0070:
0071:     public class PhantomMemoryEngine
0072:     {
0073:         public const string SystemId = "phantom_memory_engine";
...
0092:         public event Action<string, string, bool, float, float>? OnPhantomMemoryResolved;
0093:         public event Action<PhantomMemoryEngineState>? OnStateChanged;
0094:
0095:         // ── State ──────────────────────────────────────────────────────
...
0348:
0349:         public PhantomMemoryEngineState CaptureState()
0350:         {
0351:             var copy = new PhantomMemoryEngineState { systemId = _state.systemId };
...
0370:
0371:         public void RestoreState(PhantomMemoryEngineState saved)
0372:         {
0373:             if (saved == null) return;
...
0383:                     {
0384:                         CatalogDiagnostics.Warn("PhantomMemoryEngine", "RestoreState", new InvalidOperationException($"Duplicate survivorId in restore payload: {r.survivorId}"));
0385:                         return; // Reject without mutating live state
0386:                     }
```

### Current evidence: `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cf5ed2ba9337696fa5e976582a1e316d23fec999212f54f27454a888c8bc9757`
- Snapshot size: 1855 characters; 55 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011:     /// Single authoritative per-rule JSON DTO for the phantom-memory trigger
0012:     /// catalog (consumed by Phase-0 effects, PhantomMemoryEngine, and Host sessions).
0013:     /// Extended in Plan 21 with additive metadata for rich narrative anchoring.
0014:     /// </summary>
```

### Current evidence: `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e7abdb5badd964e7572d252e289b7de4a33ec010c8fada6514abd7f364dbb735`
- Snapshot size: 19646 characters; 485 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ASHFALL Core: Heirloom instance tracking, provenance accumulation, and inheritance engine (Plan 21).
0003:
0004: using System;
...
0010:     [Serializable]
0011:     public sealed class HeirloomProvenanceRecord
0012:     {
0013:         public string holder_id = string.Empty;
...
0027:         public string current_holder_id = string.Empty;
0028:         public List<HeirloomProvenanceRecord> provenance = new List<HeirloomProvenanceRecord>();
0029:         public List<int> unlocked_stages = new List<int> { 1 };
0030:         public bool is_memorialized;
...
0035:     [Serializable]
0036:     public sealed class HeirloomSystemState
0037:     {
0038:         public string systemId = HeirloomSystem.SystemId;
...
0043:     /// Engine-agnostic heirloom lifecycle manager. Manages persistent named heirlooms,
0044:     /// appends bounded provenance, handles generational inheritance on survivor death,
0045:     /// and resolves holder-specific memories.
0046:     /// </summary>
...
0049:         public const string SystemId = "heirloom_system";
0050:         public const int MaxProvenanceEntriesPerInstance = 24;
0051:
0052:         private readonly HeirloomCatalog _catalog;
...
0061:
0062:         public HeirloomSystem(HeirloomCatalog catalog, ILog? log = null)
0063:         {
0064:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
...
0153:
0154:             inst.provenance.Add(new HeirloomProvenanceRecord
0155:             {
0156:                 holder_id = initialHolderId ?? "shelter_communal",
...
0173:
0174:             // Close current provenance record
0175:             if (inst.provenance.Count > 0)
0176:             {
...
0183:
0184:             // Append new provenance record (bounded)
0185:             if (inst.provenance.Count >= MaxProvenanceEntriesPerInstance)
0186:             {
...
0189:
0190:             inst.provenance.Add(new HeirloomProvenanceRecord
0191:             {
0192:                 holder_id = string.IsNullOrEmpty(newHolderId) ? "shelter_communal" : newHolderId,
...
0224:
```

### Current evidence: `Assets/Ashfall.Core/Phantoms/HeirloomCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `fc7916235a24f30a3946ccc3f6bd8bb41b4ba20b657e719a0a69c7399fa2f4a5`
- Snapshot size: 3688 characters; 107 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ASHFALL Core: Heirloom definitions, historical stages, and catalog loader (Plan 21).
0003:
0004: using System;
0005: using System.Collections.Generic;
0006: #pragma warning disable CS8618
0007:
0008: namespace Ashfall.Core.Phantoms
0009: {
0010:     /// <summary>
0011:     /// One authored historical epoch for an heirloom (e.g. Pre-War Origin, Exchange Survival, Current).
0012:     /// </summary>
0013:     [Serializable]
0014:     public sealed class HeirloomHistoricalStage
0015:     {
0016:         public int stage_index;
0017:         public string period_label;
0018:         public string original_holder;
0019:         public string historical_fragment;
0020:     }
```

### Current evidence: `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `298c3db6a1591f12e675af437475529e374983544ef7cabca0f21e698f731c94`
- Snapshot size: 11146 characters; 287 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0021:     {
0022:         public string systemId = ConfessionSecretSystem.SystemId;
0023:         public List<string> discoveredSecretIds = new List<string>();
0024:         public List<string> resolvedSecretIds = new List<string>();
...
0027:
0028:     public sealed class ConfessionSecretSystem
0029:     {
0030:         public const string SystemId = "confession_secret_system";
...
0045:
0046:         public ConfessionSecretSystem(ConfessionSecretCatalog catalog, ILog? log = null)
0047:         {
0048:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
```

### Current evidence: `Assets/Ashfall.Core/Phantoms/ConfessionSecretCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f325798d18d19740fd2d1aa1ebcb06df2ede5f3713c075f5fc8020f368a0cbbd`
- Snapshot size: 5294 characters; 128 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ASHFALL Core: Confession & secret definitions, catalog loader, and query service (Plan 21).
0003:
0004: using System;
0005: using System.Collections.Generic;
0006: #pragma warning disable CS8618
0007:
0008: namespace Ashfall.Core.Phantoms
0009: {
0010:     [Serializable]
0011:     public sealed class ConfessionSecretEntryJson
0012:     {
0013:         public string secret_id = string.Empty;
0014:         public string archetype_id = string.Empty;
0015:         public string category = "npc_personal"; // "npc_personal", "faction_institutional", "bunker_internal", "historical_confession"
0016:         public string subject_id = string.Empty;
0017:         public string secret_title = string.Empty;
0018:         public string secret_text = string.Empty;
0019:         public string discovery_path = "direct_confession"; // "direct_confession", "document", "radio", "deathbed", "phantom_memory", "expedition"
0020:         public string discovery_source_id = string.Empty;
```

### Current evidence: `src/Host/PhantomMemoryHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c530779e66dcb2378b9d7eb92ec99a2eebaa78fdca9cdec6a9dd8aedbe17ab55`
- Snapshot size: 18210 characters; 399 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0013:     /// Thin Godot-host session for the Phantom Memory Engine (Antigravity #41).
0014:     /// Wraps PhantomMemoryEngine with demo rules, save/load, and a demo survivor.
0015:     /// </summary>
0016:     public sealed class PhantomMemoryHostSession
...
0043:
0044:         public PhantomMemoryHostSession(PhantomMemoryEngine engine = null!, bool loadDefaults = true, ISeededRng? rng = null)
0045:         {
0046:             Engine = engine ?? new PhantomMemoryEngine();
...
0086:         {
0087:             var engine = new PhantomMemoryEngine();
0088:             bool loaded = LoadRulesFromJson(engine, dataDir, fileIO, jsonSerializer, onError);
0089:             return new PhantomMemoryHostSession(engine, loadDefaults: !loaded, rng: rng);
...
0130:
0131:         public PhantomMemoryEngineState CaptureSave() => Engine.CaptureState();
0132:         public void RestoreSave(PhantomMemoryEngineState state) => Engine.RestoreState(state);
0133:
...
0327:         public static bool LoadRulesFromJson(
0328:             PhantomMemoryEngine engine,
0329:             string dataDir,
0330:             IFileIO? fileIO = null,
```

### Current evidence: `src/Host/PhantomMemorySaveStore.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `37a1ca574ecbba22ea3478dd7b3fa114c45f1dec03a6210b75a7e1130db2c204`
- Snapshot size: 2561 characters; 50 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0003: // Save Store : PhantomMemorySaveStore
0004: // Core State : Ashfall.Core.PhantomMemoryEngineState
0005: // Host Caller: Main.Phase0 / PhantomMemoryHostSession
0006: // Purpose    : Phase 0 phantom memory engine, trauma flashbacks, and psychological echoes
...
0023:
0024:         private static readonly SaveStore<PhantomMemoryEngineState> s_store =
0025:             SaveStoreHub.Checksummed<PhantomMemoryEngineState>(FileName, nameof(PhantomMemorySaveStore), allowLegacyBareState: false);
0026:
...
0031:         /// <summary>Direct aggregate capture: serialize state to JSON for the envelope.</summary>
0032:         public static string TryCaptureDirect(PhantomMemoryEngineState state) => s_store.CaptureBare(state);
0033:
0034:         /// <summary>Direct aggregate restore: deserialize state from envelope JSON.</summary>
...
0037:         /// <summary>Capture state to JSON without writing to disk.</summary>
0038:         public static string TryCapture(PhantomMemoryEngineState state) => s_store.CaptureBare(state);
0039:
0040:         /// <summary>Restore state from JSON without reading from disk.</summary>
...
0044:
0045:         public static PhantomMemoryEngineState? TryLoad() => s_store.TryLoad();
0046:
0047:         /// <summary>Capture the exact persisted bytes for the campaign envelope without writing to disk.</summary>
```

### Current evidence: `src/Main.OrphanSealWave1.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9486381260760b9e2420d8d033ad67cc7e0c23a7c0afabb120d5368675664c90`
- Snapshot size: 23378 characters; 525 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0264:                 catalog.Load(catalogJson, new SystemTextJsonSerializer());
0265:             var system = new ConfessionSecretSystem(catalog, new GodotLog());
0266:             var saved = ConfessionSecretSaveStore.TryLoad();
0267:             if (saved != null) system.RestoreState(saved);
```

### Current evidence: `src/UI/PhantomMemoryPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/PhantomMemoryPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a7c3fbce3112a3020097e97f6050682ecc3d3eab59d69776358b8175aade55f3`
- Snapshot size: 15596 characters; 334 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0244:                     {
0245:                         string? cat = PhantomMemoryEngine.GetCategoryFromId(slot.Item.id);
0246:                         if (cat != null && cat != "generic")
0247:                         {
...
0265:                         {
0266:                             _selectedItemCategory = PhantomMemoryEngine.GetCategoryFromId(itemId) ?? "relic";
0267:                             _host.InspectRelic(curSv.survivorId, itemId, out string res, consumeItem: false);
0268:                             RefreshView();
```

### Current evidence: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `496edf357dd32e7a66331bcfeefed3e7968ba099cb5d4f9f92edc644d55348a7`
- Snapshot size: 47040 characters; 885 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "background_id": "child_refugee",
0006:       "triggers": [
0007:         {
0008:           "trigger_id": "phantom_trigger_child_shoe",
0009:           "item_category": "childhood",
0010:           "item_id": "childs_mitten",
0011:           "motivation_chance": 0.3,
0012:           "description": "A single child's shoe, the synthetic sole stiff with dried mud. {name} holds it by the heel. The laces are tied in a double knot.",
0013:           "motivation_text": "{name} places the shoe on a shelf. 'Secure the perimeter,' they say, their voice flat.",
0014:           "breakdown_text": "{name} drops the shoe and stares at the concrete wall. They do not report for their next shift rotation.",
0015:           "affinity_background": "child_refugee",
0016:           "morale_payload": 15.0,
0017:           "guilt_payload": 0.0
0018:         },
0019:         {
0020:           "trigger_id": "phantom_trigger_child_photograph",
```

### Current evidence: `Assets/StreamingAssets/Data/phantom_heirlooms.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cb5f285c7bfd6c1c75a8170aea5b614b31600ba6bdf3399b9ec672584398e7e4`
- Snapshot size: 19944 characters; 527 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "heirloom_id": "heirloom_grandfathers_dosimeter",
0006:       "base_item_id": "dosimeter",
0007:       "title": "Grandfather's Dosimeter",
0008:       "origin": "Civil Defense Radiation Safety Division, Sub-Sector 4",
0009:       "is_legacy_candidate": true,
0010:       "memorial_eligible": true,
0011:       "stages": [
0012:         {
0013:           "stage_index": 1,
0014:           "period_label": "Pre-War Baseline",
0015:           "original_holder": "Technician Paul Thorne",
0016:           "historical_fragment": "Calibrated each morning against a sealed cesium check source. The needle always zeroed with mechanical obedience."
0017:         },
0018:         {
0019:           "stage_index": 2,
0020:           "period_label": "The Exchange & Bunker 7",
```

### Current evidence: `Assets/StreamingAssets/Data/confession_secrets.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3fff37f541bfff91b6265fc2596e5ec7d6fcf664f18fecdc39b883f57d9f7349`
- Snapshot size: 94904 characters; 993 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "secret_id": "secret_surgeon_lost_patient",
0006:       "archetype_id": "the_surgeon",
0007:       "category": "npc_personal",
0008:       "subject_id": "the_surgeon",
0009:       "secret_title": "The Patient They Lost",
0010:       "secret_text": "{name} stares at their hands. 'There was a patient, Day 1. Twenty-three years old. Shrapnel in the abdomen. I could have saved them. I chose to save someone else instead — someone with better odds. The twenty-three-year-old died alone in the hallway.'",
0011:       "discovery_path": "direct_confession",
0012:       "discovery_source_id": "silver_scalpel",
0013:       "gating_flag": "flag_secret_surgeon_confessed",
0014:       "forgiveness_outcome": "The other survivor takes {name}'s hands. 'You made the call a doctor has to make. The person you saved — they're probably alive because of you.' {name} exhales for what feels like the first time in months.",
0015:       "forgiveness_affinity": 20,
0016:       "forgiveness_morale": 15,
0017:       "grudge_outcome": "'You played God,' the other survivor says. 'And you have not said their age out loud once.' {name} looks away. It is not an accident of memory, and both of them now know that it is not. Nothing is settled. The corridor stays colder for a week, and the number twenty-three stays exactly where it was put.",
0018:       "grudge_affinity": -30,
0019:       "grudge_morale": -15,
0020:       "expose_outcome": "You enter it in the clinic ledger on the same page as the supply count, in the same hand: one patient, twenty-three, shrapnel to the abdomen, not selected for the theatre. No commentary. There is no name to enter, and you note that too. {name} reads it before the morning round and does not strike it out. Two of the staff stop asking {name} to check their work. A third starts asking more often, which is the harder of the two to live with.",
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

### Current evidence: `Assets/StreamingAssets/Data/characters.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cd33aa4801aade57932525194755b413d83ab2cd212c0ca7a5ccd09f011b5c58`
- Snapshot size: 68407 characters; 1838 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
1629:         "copy_discipline_lessons",
1630:         "provenance_read_aloud",
1631:         "the_record_as_it_stands"
1632:       ],
```

### Current evidence: `Assets/StreamingAssets/Data/survivors.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
- Snapshot size: 70319 characters; 1249 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:     "schema_version": 1,
0003:     "survivors": [
0004:         {
0005:             "id": "elena_vasquez",
0006:             "displayName": "Elena Vasquez",
0007:             "profession": "Paramedic",
0008:             "bio": "Nine years of night shifts in an ambulance bay taught Elena to sort the dying fast. She still checks wrists for pulses out of habit, and wears her watch strap-first over the cuff so it never catches on gloves.",
0009:             "baseHealth": 100
0010:         },
0011:         {
0012:             "id": "marcus_olejnik",
0013:             "traitIds": ["trait_claustrophobe"],
0014:             "displayName": "Marcus Olejnik",
0015:             "profession": "Mechanical Engineer",
0016:             "bio": "Marcus kept a turbine hall running on parts that were condemned five years earlier. He talks to machines more easily than to people, and is usually right about what they tell him.",
0017:             "baseHealth": 90
0018:         },
0019:         {
0020:             "id": "suki_tanaka",
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

### Current evidence: `src/UI/InventoryDetailPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `3442795d2c64a19bc1358f43a62108a6ff34fc7cfc660e979bb579f722fc2db5`
- Snapshot size: 13754 characters; 288 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0039:     private string _itemId = string.Empty;
0040:     private System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? _technicalProvenance;
0041:     private System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.LeatherworkRecord>? _leatherProvenance;
0042:
...
0047:     public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions = null, ExpansionEnrichmentCatalog? enrichment = null)
0048:         => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance: null);
0049:
0050:     /// <summary>
...
0055:     /// </summary>
0056:     public void Bind(InventoryHostSession? inventory, string itemId, ItemDescriptionCatalog? descriptions, ExpansionEnrichmentCatalog? enrichment, System.Collections.Generic.IReadOnlyList<Ashfall.Core.Narrative.TechnicalMaterialRecord>? technicalProvenance)
0057:         => Bind(inventory, itemId, descriptions, enrichment, technicalProvenance, leatherProvenance: null);
0058:
...
0061:     /// curing salt, strap leather). Display-only: archived tanning/currying
0062:     /// records render as dim provenance lines describing one historical
0063:     /// production batch; no condition/durability/trade state is read or
0064:     /// changed. Null (default) renders nothing — existing callers untouched.
...
0071:         _enrichment = enrichment ?? inventory?.EnrichmentCatalog;
0072:         _technicalProvenance = technicalProvenance;
0073:         _leatherProvenance = leatherProvenance;
0074:         RefreshView();
...
0146:
0147:         // ── Technical provenance (Plan 158, display-only) ──
0148:         if (_technicalProvenance != null && _technicalProvenance.Count > 0)
0149:         {
...
0159:
0160:         // ── Leather provenance (Plan 159, display-only) ──
0161:         if (_leatherProvenance != null && _leatherProvenance.Count > 0)
0162:         {
```

### Current evidence: `src/UI/EpiloguePanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/EpiloguePanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `40e17093add485b4ca0e323093811deeabc7afdee6e5751975926c2ead73c9ba`
- Snapshot size: 10667 characters; 251 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0144:
0145:             contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("AUTHORITATIVE CAMPAIGN PROVENANCE & EVALUATION TRACE"));
0146:
0147:             var traceCard = AshfallUiHelpers.MakePanel();
```

### Current evidence: `Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `20b7c39616f617f717d9c5cb49550e396ad21c3a3d2d6c97345f1c128cb84e3c`
- Snapshot size: 16897 characters; 405 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public class PhantomMemoryEngineTests
0009:     {
0010:         [Fact]
...
0037:         {
0038:             var engine = new PhantomMemoryEngine();
0039:             engine.RegisterRule("generic", "photograph", 0.5f, "desc");
0040:
...
0054:         {
0055:             var engine = new PhantomMemoryEngine();
0056:             engine.TriggerChanceOverride = 1.0f;
0057:             engine.RegisterRule("teacher", "correspondence", 1.0f, "desc", "motivation", "breakdown");
...
0080:         {
0081:             var engine = new PhantomMemoryEngine();
0082:             engine.RegisterRule("nurse", "medical", 0.5f, "desc",
0083:                 "{name} the medic is inspired.", "{name} the medic breaks down.");
...
0102:         {
0103:             var engine = new PhantomMemoryEngine();
0104:             engine.TriggerChanceOverride = 1.0f;
0105:             engine.RegisterRule("child_refugee", "childhood", 1.0f, "desc");
...
0117:             var state = engine.CaptureState();
0118:             var engineB = new PhantomMemoryEngine();
0119:             engineB.TriggerChanceOverride = 1.0f;
0120:             engineB.RegisterRule("child_refugee", "childhood", 1.0f, "desc");
...
0129:         {
0130:             var engine = new PhantomMemoryEngine();
0131:             engine.TriggerChanceOverride = 1.0f;
0132:             engine.RegisterRule("former_soldier", "military", 1.0f, "desc");
...
0162:         {
0163:             var engine = new PhantomMemoryEngine();
0164:             engine.TriggerChanceOverride = 1.0f;
0165:             engine.RegisterRule("generic", "personal_item", 1.0f, "desc");
...
0181:         {
0182:             var engine = new PhantomMemoryEngine();
0183:             engine.TriggerChanceOverride = 1.0f;
0184:             engine.RegisterRule("nurse", "medical", 1.0f, "desc", "motivated", "breakdown");
...
0189:
0190:             Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus,
0191:                 engine.GetWorkEfficiencyMultiplier("sv_mot"), 4);
0192:
...
0199:         {
0200:             var engine = new PhantomMemoryEngine();
0201:             engine.TriggerChanceOverride = 1.0f;
0202:             // motivationChance 0 → any trigger is a breakdown.
...
```

### Current evidence: `Ashfall.Core.Tests/HeirloomSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `390762863ccb18a4995e02cdcc9e0307b891d98c90474694442e05b094286496`
- Snapshot size: 8241 characters; 218 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011: {
0012:     public class HeirloomSystemTests
0013:     {
0014:         private readonly string _dataDir;
...
0031:             var catalog = new HeirloomCatalog();
0032:             string filePath = Path.Combine(_dataDir, "phantom_heirlooms.json");
0033:             if (File.Exists(filePath))
0034:             {
...
0055:         [Fact]
0056:         public void HeirloomSystem_CreateInstance_InitializesHolderAndProvenance()
0057:         {
0058:             var catalog = CreateLoadedCatalog();
...
0064:             Assert.Contains(1, inst.unlocked_stages);
0065:             Assert.Single(inst.provenance);
0066:             Assert.Equal("survivor_01", inst.provenance[0].holder_id);
0067:             Assert.Equal("initial_discovery", inst.provenance[0].transfer_reason);
...
0070:         [Fact]
0071:         public void HeirloomSystem_TransferHeirloom_AppendsProvenance()
0072:         {
0073:             var catalog = CreateLoadedCatalog();
...
0080:             Assert.Equal("survivor_02", inst.current_holder_id);
0081:             Assert.Equal(2, inst.provenance.Count);
0082:             Assert.Equal("direct_trade", inst.provenance[1].transfer_reason);
0083:         }
...
0088:             var catalog = CreateLoadedCatalog();
0089:             var system = new HeirloomSystem(catalog);
0090:             var succEngine = new GenerationalSuccessionEngine();
0091:             var lineage = new GenerationalLineageExtension(succEngine);
...
0107:         [Fact]
0108:         public void HeirloomSystem_HandleSurvivorDeath_FallsBackToTrustBondWhenNoKin()
0109:         {
0110:             var catalog = CreateLoadedCatalog();
...
0130:         [Fact]
0131:         public void HeirloomSystem_HandleSurvivorDeath_FallsBackToCommunalStorageWhenNoEligible()
0132:         {
0133:             var catalog = new HeirloomCatalog();
...
0148:         [Fact]
0149:         public void HeirloomSystem_BoundedProvenance_CapsHistoryAtLimit()
0150:         {
0151:             var catalog = new HeirloomCatalog();
...
0160:
0161:             Assert.Equal(HeirloomSystem.MaxProvenanceEntriesPerInstance, inst.provenance.Count);
0162:             Assert.Equal(24, inst.provenance.Count);
0163:             Assert.Equal("holder_35", inst.current_holder_id);
...
```

### Current evidence: `Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9c76b7b7ad674be8a9d6b3acc41ec9ca247a44bb64a0b41914465f930dffaad6`
- Snapshot size: 4222 characters; 84 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.IO;
0004: using Ashfall.Core.Narrative;
0005: using Xunit;
0006:
0007: namespace Ashfall.Core.Tests
0008: {
0009:     public sealed class DwellerHeirloomCatalogTests
0010:     : CatalogTestBase{
0011:         private static string FindDataDir()
0012:         {
0013:             string start = Directory.GetCurrentDirectory();
0014:             if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
0015:             if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
0016:             throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
0017:         }
0018:
0019:         [Fact]
0020:         public void DwellerHeirlooms_LoadsAll30CanonicalKeepsakes()
```

### Current evidence: `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a6b9f1e9c9acd58cbdce32abff6bba82c39fc512f64cce4f1ae8f51f7a6c07b5`
- Snapshot size: 14103 characters; 334 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0010: {
0011:     public class ConfessionSecretSystemTests
0012:     {
0013:         private readonly string _dataDir;
...
0056:         [Fact]
0057:         public void ConfessionSecretSystem_DiscoverSecret_RegistersState()
0058:         {
0059:             var catalog = CreateLoadedCatalog();
...
0068:         [Fact]
0069:         public void ConfessionSecretSystem_ExposeSecret_AppliesFactionAndGuilt()
0070:         {
0071:             var catalog = CreateLoadedCatalog();
...
0098:         [Fact]
0099:         public void ConfessionSecretSystem_BlackmailSecret_AppliesHardening()
0100:         {
0101:             var catalog = CreateLoadedCatalog();
...
0118:         [Fact]
0119:         public void ConfessionSecretSystem_KeepSecret_AppliesTrust()
0120:         {
0121:             var catalog = CreateLoadedCatalog();
...
0141:         [Fact]
0142:         public void ConfessionSecretSystem_ResolveInterpersonal_ForgivenessAndGrudge()
0143:         {
0144:             var catalog = CreateLoadedCatalog();
...
0184:         [Fact]
0185:         public void ConfessionSecretSystem_IdempotentLeverageResolution_PreventsDoubleAction()
0186:         {
0187:             var catalog = CreateLoadedCatalog();
...
0200:         [Fact]
0201:         public void ConfessionSecretSystem_CaptureAndRestoreState_Roundtrips()
0202:         {
0203:             var catalog = CreateLoadedCatalog();
...
0210:
0211:             var systemB = new ConfessionSecretSystem(catalog);
0212:             systemB.RestoreState(state);
0213:
...
0272:         [Fact]
0273:         public void ConfessionSecretSystem_Plan88NewSecrets_ResolveInterpersonalAndLeverage()
0274:         {
0275:             var catalog = CreateLoadedCatalog();
```

### Current evidence: `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cbc3f8c2a900f56bee98fb407f23c3c513e576db317f12f98b03d51ac7ebc0de`
- Snapshot size: 6874 characters; 161 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0014:         [Fact]
0015:         public void MemorialSystem_WithEulogyEngine_ComposesAndPersistsEulogy()
0016:         {
0017:             var eulogyEngine = new ProceduralEulogyEngine();
...
0054:             var captured = system.CaptureState();
0055:             var restoredSystem = new MemorialSystem(new MemorialState());
0056:             restoredSystem.RestoreState(captured);
0057:
...
0062:         [Fact]
0063:         public void HeirloomSystem_HolderModifiers_CalculatedAndBounded()
0064:         {
0065:             var catalog = new HeirloomCatalog();
...
0086:             catalog.Load(json, serializer);
0087:             var system = new HeirloomSystem(catalog);
0088:
0089:             // Create instances and assign to holder
```

### Current evidence: `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5f5bfedc2b887f678937777fb761878e21c152db2d95a2584826e5ecca5af222`
- Snapshot size: 12504 characters; 274 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0018:         {
0019:             // 1. MemorialSystem with ProceduralEulogyEngine
0020:             var eulogyEngine = new ProceduralEulogyEngine();
0021:             var memorialState = new MemorialState();
...
0048:
0049:             var entry = memorialSystem.Memorialize(memorialInput);
0050:             Assert.NotNull(entry);
0051:             Assert.False(string.IsNullOrEmpty(entry.EulogyText));
...
0056:             // Save/Restore roundtrip preserves eulogy
0057:             var memorialCapture = memorialSystem.CaptureState();
0058:             var restoredMemorial = new MemorialSystem(new MemorialState());
0059:             restoredMemorial.RestoreState(memorialCapture);
...
0062:
0063:             // 2. HeirloomSystem: holder morale and fatigue relief
0064:             var heirloomCatalog = new HeirloomCatalog();
0065:             var serializer = new SystemTextJsonSerializer();
...
0079:
0080:             var heirloomSystem = new HeirloomSystem(heirloomCatalog);
0081:             heirloomSystem.CreateInstance("item_heirloom_brass_monocular", "dweller_protege", 150);
0082:
...
0137:             };
0138:             validCatalog.ConsumerSystems.Add("HeirloomSystem");
0139:             validCatalog.ConsumerSystems.Add("NeedsSystem");
0140:
...
0183:             };
0184:             heirloomCatalogEntry.ConsumerSystems.Add("HeirloomSystem");
0185:
0186:             var acceptance = ContentAcceptancePipeline.Evaluate(heirloomCatalogEntry);
...
0220:             // Step 3 & 4: Memorialize fallen traveler
0221:             var memorialSystem = new MemorialSystem(new MemorialState())
0222:             {
0223:                 EulogyEngine = new ProceduralEulogyEngine()
...
0261:
0262:             var heirloomSys = new HeirloomSystem(catalog);
0263:             heirloomSys.CreateInstance("item_heirloom_scout_compass", "dweller_apprentice_tara", 85);
0264:             Assert.Equal(6.0f, heirloomSys.GetHolderMoraleModifier("dweller_apprentice_tara"));
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/phantom_triggers.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=20; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `496edf357dd32e7a66331bcfeefed3e7968ba099cb5d4f9f92edc644d55348a7`
#### `Assets/StreamingAssets/Data/phantom_heirlooms.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=12; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `cb5f285c7bfd6c1c75a8170aea5b614b31600ba6bdf3399b9ec672584398e7e4`
#### `Assets/StreamingAssets/Data/confession_secrets.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=38; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3fff37f541bfff91b6265fc2596e5ec7d6fcf664f18fecdc39b883f57d9f7349`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
#### `Assets/StreamingAssets/Data/characters.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=84; sample IDs=['npc_bram_ostrowski', 'npc_sergeant_pell', 'npc_doctor_ianov', 'npc_wren', 'npc_kestrel', 'npc_nomi_fisk', 'npc_ivor_lasko', 'npc_the_cartwright_sisters']
- `schema_version`: `1`
- SHA-256: `cd33aa4801aade57932525194755b413d83ab2cd212c0ca7a5ccd09f011b5c58`
#### `Assets/StreamingAssets/Data/survivors.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, survivors`
- `survivors`: list count=129; sample IDs=['elena_vasquez', 'marcus_olejnik', 'suki_tanaka', 'the_surgeon', 'the_pharmacist', 'the_vet', 'the_therapist', 'the_undertaker']
- `schema_version`: `1`
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
## Symbol and caller audit

#### `PhantomMemoryEngine` — HOST_REFERENCE_PRESENT — core/declaration=6, host=17, test=43
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs:46` (core) — public string systemId = PhantomMemoryEngine.SystemId;
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs:71` (declaration) — public class PhantomMemoryEngine
- `Assets/Ashfall.Core/PhantomMemoryEngine.cs:384` (core) — CatalogDiagnostics.Warn("PhantomMemoryEngine", "RestoreState", new InvalidOperationException($"Duplicate survivorId in restore payload: {r.survivorId}"));
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:428` (core) — ["phantom_triggers.json"] = new[] { "PhantomMemoryHostSession", "PhantomMemoryEngine" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1034` (core) — ["phantom_triggers.json"] = new[] { "PhantomMemoryHostSession", "PhantomMemoryEngine" },
- `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs:12` (core) — /// catalog (consumed by Phase-0 effects, PhantomMemoryEngine, and Host sessions).
- `src/Host/Phase0HostSession.cs:234` (host) — public PhantomMemoryEngine Phantom { get; }
- `src/Host/Phase0HostSession.cs:330` (host) — Phantom = new PhantomMemoryEngine();
- `src/Host/Phase0HostSession.cs:336` (host) — Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.MotivationMoraleBoost);
- `src/Host/Phase0HostSession.cs:342` (host) — Consumers.ApplyMoraleDelta?.Invoke(sv, PhantomMemoryEngine.BreakdownMoraleDrop);
- `src/Host/Phase0HostSession.cs:343` (host) — Consumers.ApplyWorkRefusalHours?.Invoke(sv, PhantomMemoryEngine.BreakdownWorkRefusalHours);
- `src/Host/PhantomMemoryHostSession.cs:14` (host) — /// Wraps PhantomMemoryEngine with demo rules, save/load, and a demo survivor.
- `src/Host/PhantomMemoryHostSession.cs:18` (host) — public PhantomMemoryEngine Engine { get; }
- `src/Host/PhantomMemoryHostSession.cs:44` (host) — public PhantomMemoryHostSession(PhantomMemoryEngine engine = null!, bool loadDefaults = true, ISeededRng? rng = null)
- `src/Host/PhantomMemoryHostSession.cs:46` (host) — Engine = engine ?? new PhantomMemoryEngine();
- `src/Host/PhantomMemoryHostSession.cs:87` (host) — var engine = new PhantomMemoryEngine();
- `src/Host/PhantomMemoryHostSession.cs:328` (host) — PhantomMemoryEngine engine,
- `src/Host/HostCli.PanelTests.cs:2535` (host) — Check(workMult == 1f || workMult == 1f + Ashfall.Core.PhantomMemoryEngine.MotivationWorkSpeedBonus,
- `src/Host/HostCli.PanelTests.cs:2548` (host) — var phantomHost = new PhantomMemoryHostSession(new Ashfall.Core.PhantomMemoryEngine(), loadDefaults: true, rng: new Ashfall.Core.SeededRng(42));
- `src/Host/HostCli.PanelTests.cs:2652` (host) — new Ashfall.Core.PhantomMemoryEngine(),
- `src/Host/HostCli.PanelTests.cs:2664` (host) — new Ashfall.Core.PhantomMemoryEngine(),
- `src/UI/PhantomMemoryPanel.cs:245` (host) — string? cat = PhantomMemoryEngine.GetCategoryFromId(slot.Item.id);
- `src/UI/PhantomMemoryPanel.cs:266` (host) — _selectedItemCategory = PhantomMemoryEngine.GetCategoryFromId(itemId) ?? "relic";
- `Ashfall.Core.Tests/PhantomMemoryEngineTests.cs:13` (test) — var engine = new PhantomMemoryEngine();
- … 42 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `HeirloomSystem` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=5, host=0, test=16
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:429` (core) — ["phantom_heirlooms.json"] = new[] { "HeirloomCatalog", "HeirloomSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1035` (core) — ["phantom_heirlooms.json"] = new[] { "HeirloomSystem", "GenerationalLineageExtension", "SurvivorRelationsSystem" },
- `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs:38` (core) — public string systemId = HeirloomSystem.SystemId;
- `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs:47` (declaration) — public sealed class HeirloomSystem
- `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs:62` (core) — public HeirloomSystem(HeirloomCatalog catalog, ILog? log = null)
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:59` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:74` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:89` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:111` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:134` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:152` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:161` (test) — Assert.Equal(HeirloomSystem.MaxProvenanceEntriesPerInstance, inst.provenance.Count);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:174` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:198` (test) — var systemA = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:207` (test) — var systemB = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs:87` (test) — var system = new HeirloomSystem(catalog);
- `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs:63` (test) — // 2. HeirloomSystem: holder morale and fatigue relief
- `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs:80` (test) — var heirloomSystem = new HeirloomSystem(heirloomCatalog);
- `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs:138` (test) — validCatalog.ConsumerSystems.Add("HeirloomSystem");
- `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs:184` (test) — heirloomCatalogEntry.ConsumerSystems.Add("HeirloomSystem");
- `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs:262` (test) — var heirloomSys = new HeirloomSystem(catalog);
#### `ConfessionSecretSystem` — HOST_REFERENCE_PRESENT — core/declaration=3, host=7, test=15
- `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs:22` (core) — public string systemId = ConfessionSecretSystem.SystemId;
- `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs:28` (declaration) — public sealed class ConfessionSecretSystem
- `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs:46` (core) — public ConfessionSecretSystem(ConfessionSecretCatalog catalog, ILog? log = null)
- `src/Main.ContentCertification.cs:77` (host) — session.MarkConsumerActive("ConfessionSecretSystem", _confessionSecrets != null);
- `src/Main.OrphanSealWave1.cs:265` (host) — var system = new ConfessionSecretSystem(catalog, new GodotLog());
- `src/Host/HostCli.OrphanSealWave1.cs:181` (host) — var confessions = new ConfessionSecretSystem(confessionCatalog);
- `src/Host/HostCli.OrphanSealWave1.cs:183` (host) — var confessionsReloaded = new ConfessionSecretSystem(confessionCatalog);
- `src/Host/ContentCertificationHostSession.cs:57` (host) — new ContentCertificationFamily("confession_ritual_records", "SMALL RITUAL / COLLECTION / TRADE", "confession_secrets.json", "ConfessionSecretSystem"),
- `src/Host/OrphanSealWave1HostSessions.cs:579` (host) — public ConfessionSecretSystem System { get; }
- `src/Host/OrphanSealWave1HostSessions.cs:581` (host) — public ConfessionSecretHostSession(ConfessionSecretSystem system)
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:60` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:72` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:102` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:122` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:145` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:188` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:205` (test) — var systemA = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:211` (test) — var systemB = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs:276` (test) — var system = new ConfessionSecretSystem(catalog);
- `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs:85` (test) — var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
- `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs:146` (test) — var confessionSystem = new ConfessionSecretSystem(confessionCatalog);
- `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs:207` (test) — var originalSystem = new ConfessionSecretSystem(confessionCatalog);
- `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs:223` (test) — var restoredSystem = new ConfessionSecretSystem(confessionCatalog);
- `Ashfall.Core.Tests/Integration/OrphanSealPriorityWave1Tests.cs:308` (test) — var system = new ConfessionSecretSystem(catalog);
- … 1 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `MemorialSystem` — HOST_REFERENCE_PRESENT — core/declaration=27, host=13, test=64
- `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs:11` (core) — // MemorialSystem. It exposes:
- `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs:119` (core) — private readonly MemorialSystem? _memorial;
- `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs:136` (core) — MemorialSystem? memorial = null,
- `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:138` (core) — MemorialSystem? memorial)
- `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:257` (core) — /// Bridges directly from MemorialSystem.OnMemorialized.
- `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:289` (core) — /// Connects this archive system to listen to deaths recorded by MemorialSystem.
- `Assets/Ashfall.Core/Shelter/ShelterArchiveSystem.cs:291` (core) — public void AttachMemorialSystem(MemorialSystem memorialSystem, Func<string, string>? nameLookup = null)
- `Assets/Ashfall.Core/Survivors/MemorialComponentStore.cs:172` (core) — /// legacy MemorialSystem's first-entry-wins behavior.
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:120` (core) — private readonly MemorialSystem _memorial;
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:144` (core) — MemorialSystem memorial = null,
- `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs:324` (core) — // 6. Memorial (idempotent by survivor id inside MemorialSystem).
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs:59` (core) — /// Dead and remembered. Backed by <c>MemorialSystem</c>'s idempotent
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:490` (core) — ["memorials_expansion_05.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:500` (core) — ["wall_carving_templates.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:501` (core) — ["wasteland_grave_epitaphs.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:819` (core) — ["memorials_expansion_05.json"] = "MemorialSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:829` (core) — ["wall_carving_templates.json"] = "MemorialSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:830` (core) — ["wasteland_grave_epitaphs.json"] = "MemorialSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1096` (core) — ["memorials_expansion_05.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1106` (core) — ["wall_carving_templates.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1107` (core) — ["wasteland_grave_epitaphs.json"] = new[] { "MemorialSystem" },
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:11` (core) — /// inside <see cref="MemorialSystem"/> via the <see cref="IGriefSink"/>
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:132` (declaration) — public sealed class MemorialSystem
- `Assets/Ashfall.Core/Memorial/MemorialSystem.cs:167` (core) — public MemorialSystem(MemorialState state)
- … 80 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `phantom_heirlooms` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=4, host=0, test=2
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:78` (core) — "phantom_triggers.json", "phantom_heirlooms.json", "archive_inks.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:429` (core) — ["phantom_heirlooms.json"] = new[] { "HeirloomCatalog", "HeirloomSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:736` (core) — ["phantom_heirlooms.json"] = "HeirloomCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1035` (core) — ["phantom_heirlooms.json"] = new[] { "HeirloomSystem", "GenerationalLineageExtension", "SurvivorRelationsSystem" },
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:32` (test) — string filePath = Path.Combine(_dataDir, "phantom_heirlooms.json");
- `Ashfall.Core.Tests/HeirloomSystemTests.cs:170` (test) — string filePath = Path.Combine(_dataDir, "phantom_heirlooms.json");
#### `provenance` — HOST_REFERENCE_PRESENT — core/declaration=130, host=45, test=47
- `Assets/Ashfall.Core/SickListSystem.cs:24` (core) — /// <summary>Origin id for provenance; it is not a progression input.</summary>
- `Assets/Ashfall.Core/WaterborneExposureRules.cs:23` (core) — /// provenance in the disease engine's history).</summary>
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:60` (core) — /// stock is generated from <c>regionalSupply</c> provenance via
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs:138` (core) — /// <c>regionalSupply</c> provenance (RegionalSupplyRouter) — the data
- `Assets/Ashfall.Core/Economy/GoodsCatalog.cs:85` (core) — public string regionalSupply = string.Empty; // optional production-source provenance (Plan 56 follow-up)
- `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs:9` (core) — /// Plan 56 phase 3 — makes <c>regionalSupply</c> provenance functionally
- `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs:12` (core) — /// coastal / general); this router turns that provenance into market
- `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs:155` (core) — /// Plan 56 phase 5 — world-level provenance scale for a scarcity
- `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs:198` (core) — /// Plan 56 phase 4 — accessibility-safe provenance label for market UI
- `Assets/Ashfall.Core/Economy/RegionalSupplyRouter.cs:267` (core) — /// <summary>One provenance-derived caravan cargo lot (Plan 56 phase 3).</summary>
- `Assets/Ashfall.Core/Economy/MarketSystem.cs:137` (core) — /// <summary>Plan 18C — authored GoodDefinition.regionalSupply provenance factor.</summary>
- `Assets/Ashfall.Core/Expeditions/DiscoveryConsequenceSystem.cs:130` (core) — /// method only projects that fact into consequence provenance. The
- `Assets/Ashfall.Core/Expeditions/ExpeditionAggregate.cs:28` (core) — /// <summary>Plan 133 — consequence provenance projected from the
- `Assets/Ashfall.Core/Inventory/IPlayerInventoryPort.cs:9` (core) — /// against the authoritative player inventory. Supports atomic transactions and provenance.
- `Assets/Ashfall.Core/Inventory/InventoryProvenance.cs:22` (core) — /// Records the provenance and causality of a single inventory mutation.
- `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs:630` (core) — result.AddInfo($"Duplicate item id '{dto.id}' encountered in '{path}'. Primary definition retained (provenance collision).");
- `Assets/Ashfall.Core/Inventory/ItemInspectionModel.cs:57` (core) — ItemProvenanceChain? provenance = null)
- `Assets/Ashfall.Core/Inventory/ItemInspectionModel.cs:119` (core) — Provenance = provenance;
- `Assets/Ashfall.Core/Inventory/ItemInspectionModel.cs:146` (core) — var provenance = !string.IsNullOrEmpty(itemInstanceId) && loreSystem != null
- `Assets/Ashfall.Core/Inventory/ItemInspectionModel.cs:149` (core) — return new ItemInspectionModel(def, entry, tags, provenance);
- `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs:98` (core) — var provenance = new ItemProvenanceChain
- `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs:111` (core) — provenance.OwnershipChain.Add(crafterId!.Trim());
- `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs:112` (core) — AddLoreInternal(provenance, LoreTriggerType.Crafting, $"Crafted by {crafterId} on Day {craftingDay}.", craftingDay, crafterId);
- `Assets/Ashfall.Core/Inventory/ItemLoreSystem.cs:116` (core) — AddLoreInternal(provenance, LoreTriggerType.Discovery, $"Discovered in {discoveryLocationId} on Day {discoveryDay} ({context}).", discoveryDay, locationId: discoveryLocationId);
- … 198 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
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
#### authority lines 131-134
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
00132: | C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
00133: | C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
00134: | C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
#### authority lines 697-700
00697:
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00699:
00700: **A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 948-951
00948:
00949: **DM-9 — Survivors and interiority (C9).** Owners: needs, health, skills, traits, mental arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, spiritual rituals, memorial rites, final wishes, belongings, memory decay, phantom memory, lineage, cohorts, apprenticeships. Live catalogs: `survivors`, `skills`, `development_traits`, `mental_arcs`, `psychological_trauma`, `psychological_therapies`, `guilt_sources`, `confession_secrets`, `belief_movements`, `spiritual_rituals`, `memorial_rites`, `final_wishes`, `companion_animals`, `phantom_heirlooms`, `phantom_triggers`, `starting_survivors`, `starting_survivor_cohorts`, `expansion_survivor_fields`. Hosts: Survivors, SurvivorRelations, PsychologyArc, MentalHealthCrisis, Caregiving, Spiritual, PhantomMemory. Openings: A-21, A-22, A-23, B-04, B-14, B-15, D-02. Known caution: `ClaimPersonalBelonging` no-caller finding (unverified at runtime — re-verify before extending).
00950:
00951: **DM-10 — Quests and moral choice (C10).** Owners: questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip/quests (five split catalogs live), branching faction quests, bureaucratic morality, massive expansion corpus, repeatable quests, templates, domain questlines (dose, year-of-ash, holdfast, crossing, thirdonary, verdict, expansion). Live catalogs: `questline_master`, `dynamic_questlines`, `personal_quests`, `npc_arcs`, `quests_npc_arcs`, `moral_choice_chains/flags/gossip/quests/quests_branching/quests_distress/quests_expansion`, `quests_faction_branching`, `quests_bureaucratic_morality`, `quests_massive_expansion_200`, `quests_moral_branching_expansion`, `repeatable_quests`, `quest_templates`. Hosts: NarrativeQuestline, PersonalQuest, MoralChoice, DynamicQuestline, ExpansionQuest, NpcArc. Openings: A-24, A-25, B-16, B-17, D-07, G-01, plus the F-001 flagship.
#### authority lines 1126-1129
01126:
01127: - 2026-09-25 — Volume 5: prose specification library part 2 (twenty-one additional worked genre contracts: journal, unsent letter, communiqué, directive, liturgy, cipher, dispatch/debrief, graffiti, folklore, almanac, gazetteer, treaty, permit, glitch report, provenance, eulogy, rumor, environmental clue, item inspection, relationship reaction, quest/outcome texts, codex, map texts, world-state notification; genre list now fully covered) — ~17,700 — cumulative ~143,000
01128: - 2026-09-25 — Volume 6: twenty full subject plans expanded from Lane A seeds (FP-A01…FP-A27 selection, with wave sequencing F6-1 through F6-4) — ~16,600 — cumulative ~160,000
01129: - 2026-09-25 — Volume 7: catalog authoring contract library (field-inventory protocol, six CANON record contracts, item authoring rules, integrity checklist, schema-sheet registry with priority inventory order) — ~8,700 — cumulative ~169,000
#### authority lines 1183-1186
01183: length: 80-160 words
01184: must_include: one concrete shared memory, one unfinished practical matter
01185: must_not_include: sentimentality, apology speeches
01186: model:
#### authority lines 1390-1393
01390:
01391: ## 5.15 Provenance dossier (memory register)
01392:
01393: ` ` `text
#### authority lines 1467-1470
01467: length: 30-70 words
01468: must_include: material, one provenance hint, one use-truth
01469: must_not_include: lore dumps, stats
01470: model:
#### authority lines 1581-1584
01581:
01582: With Volumes 4 and 5, all genres in the v1.0 Part 8.4 list now carry worked contracts: manifest, audit/assay, titration record, log, journal, diary, letter (sent/unsent), intake interview, therapy note, casebook, court verdict, wiretap transcript, communiqué, directive, liturgy, hymnal (liturgy family), canon (religious), epitaph, eulogy, burial record, provenance dossier, rundown, scriptbook, cipher, dispatch, debrief, field report, waypoint note, planning brief, schedule notice, graffiti, carving, folklore (children's and adult), song (folklore family), almanac entry, gazetteer entry, bestiary entry (natural-history family, A-29 model), genealogy (lineage registers), treaty protocol, permit, load-shed schedule, maintenance glitch report, risk-of-failure wishlist (planning-brief family). Sessions extend these; they do not invent parallels.
01583:
01584:
#### authority lines 1640-1643
01640: Lane A · C2/C9 · Status PROPOSAL.
01641: Subject: fourth batch keyed to guilt sources, insomnia states, and confession secrets added since batch 3.
01642: Premise evidence: VERIFIED batches 1–3 live; VERIFIED `guilt_sources.json`, `confession_secrets.json` live.
01643: Must not change: psychological systems' state mechanics; notes are texture anchored to implemented states.
#### authority lines 1728-1731
01728: Lane A · C7 · Status PROPOSAL.
01729: Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage.
01730: Premise evidence: VERIFIED standing-record family live; VERIFIED standing records feed the Reckoning (Part 16.6).
01731: Must not change: standing-record data semantics; testimony obeys information-flow legality (a witness testifies only to what they experienced).
#### authority lines 1769-1772
01769:
01770: ## FP-A21 — Phantom-Memory Triggers for Surviving Cohorts
01771:
01772: Lane A · C9 · Status PROPOSAL.
#### authority lines 1815-1818
01815:
01816: Twenty seeds expanded (A-01 through A-27 selection; the remaining Lane A seeds — A-09, A-14, A-15, A-20, A-23, A-24, A-25, A-28, A-29, A-30 — await either session premise reads or consumption by flagship plans F-003, F-005, A-25's audit, and corpus sweeps). Recommended wave structure for implementation sessions: Wave F6-1 (FP-A01, FP-A02, FP-A06, FP-A27 — shelter/food/weather documents); Wave F6-2 (FP-A03, FP-A04, FP-A05 — medical documents); Wave F6-3 (FP-A16, FP-A17, FP-A18, FP-A19 — faction/radio documents); Wave F6-4 (FP-A12, FP-A13, FP-A21, FP-A22, FP-A26 — travel/people/economy documents); FP-A08, FP-A10, FP-A11 slot into whichever wave their premise reads land in. One lane per wave holds: all are Lane A, so waves remain prose-only and data-first, per the rotation discipline.
01817:
01818:
#### authority lines 2254-2257
02254: Route: CORE-EXTENSION downstream of the sealed branch (exposure evaluation through the disease owner; rite affordances through the sealed memorial path). Determinism: existing streams. Save impact: EXISTING-SECTION.
02255: Continuity: exposure respects pathogen vocabulary; rites respect the vigil mapping (DR-11); the dead appear only in memory contexts (hard world rule).
02256: Verification: focused disease tests (remains-handling exposure cases); memorial pipeline tests untouched-and-green (regression proof that the sealed path was not altered); determinism proof.
02257: Open premises: read the remains-handling downstream surface; obtain the sealed-surface owner's coordination note before implementation.
#### authority lines 3282-3285
03282: ## 17.3 C3 — Water, food, agriculture (DM-3)
03283: Owns: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Expanded plans: FP-A06, FP-A07, FP-A08; satellites B-05, C-12, F-012. Phase I: preservation/grain assay twins, cellar and silo follow-ons, apiculture continuation. Phase II: preservation-contamination bridge (B-05, zoonosis model) and the F-012 dive-site/hydroponic audit's follow-on tranches. Phase III: greenhouse/aeroponics economics harness (C-12); seasonal-calendar prose depth across all cultivation families. Phase IV: DR entry if F-012's audit finds the dive/hydroponic domains already partly mapped. Multi-year arc: the food chain legible end to end, from cultivar to kitchen, with assay prose as its memory.
03284:
03285: ## 17.4 C4 — Power and industry (DM-4)
#### authority lines 3299-3302
03299:
03300: ## 17.9 C9 — Survivors and interiority (DM-9)
03301: Owns: needs, health, skills, traits, arcs, trauma, therapies, guilt, crises, morale contagion, relations, caregiving, dependency, companion animals, beliefs, rituals, rites, final wishes, belongings, memory, phantom memory, lineage, cohorts, apprenticeships. Expanded plans: FP-A21, FP-A22, FP-A23 (this factory); satellites B-04, B-14, B-15, D-02. Phase I: heirloom-trigger expansion keyed to surviving cohorts; final-wishes document twins; intake continuation. Phase II: B-15 rite evidence enrollment (vocabulary check first); D-02 lineage horizon extension toward Day 3650. Phase III: guilt-source and confession prose depth census; cohort survival state as a conditioning axis for any new document genre. Phase IV: DR entry if the ClaimPersonalBelonging no-caller finding resolves (re-verify before extending, per DM-9's caution). Multi-year arc: the shelter as a community of records — every survivor's interiority documentable, every loss leavable behind as a paper trace.
03302:
#### authority lines 3427-3430
03427:
03428: PR #62's listing describes a full sync of local workplace with upstream integrating Wave 5 survivor, shelter, narrative, economy, spiritual, treaty, UI, persistence, and data-authority changes, and names Core systems the factory's deep maps do not contain: `MemoryDecaySystem` (Plan 185), `ShelterArchiveSystem` and `ShelterAtmosphereSystem` (Plan 162), `TimeCapsuleSystem`, `CultureCreationSystem`, `DocumentationSystem`, `ResourceRationingSystem`, `ColonySystem`, `DiscoveryConsequenceSystem`, `CartographySystem`, `RumorSystem`, `ItemLoreSystem`, `AfflictionDutyBridge`, `PropagandaSystem`, `DynamicQuestGenerator`, `RegionalTreatyCatalogLoader`, `ShelterNoiseSystem`, `ShelterSecuritySystem`, `ChildDevelopmentSystem`, `ExerciseSystem`, `HiddenAgendaSystem`, `HobbySystem`, `InterpersonalConflictSystem`, plus a fifteen-plan completion-first Seal-steps program authorized 2026-09-18 and Plan 24's signed closeout options. PR #62's own merge state is not stated in the listing; however, PR #66 (merged 2026-09-19) is listed as integrating Plans 167 (tunnels) and 219 (documentation) with focused tests 8/8 and 6/6, synchronizing port contract policies, the CLI command catalog, the save store contract matrix, and agent rulebooks, and recording 15/15 partial plans integrated — which corroborates that the wave-5/6 partial-integration program reached main through a different carrier.
03429: Status: subsystem list VERIFIED-AS-LISTED; carrier merge state UNVERIFIED; corroborating PR #66 MERGED-AS-LISTED.
03430: Consequences: this is the largest deep-map gap the re-audit has found. DM-9 gains memory-decay, child-development, exercise, hidden-agenda, hobby, and interpersonal-conflict systems; DM-1 gains shelter archive, atmosphere (already known), noise, and security systems; DM-11 gains resource rationing; DM-6 gains cartography; DM-8 gains rumor (already known as bands) and propaganda; DM-10 gains a dynamic quest generator; DM-13 gains time capsules and discovery consequences. A working-tree session must inventory these before any cluster's Phase I census, because several censi (E-01's briefing enumeration, A-31/A-32's domain checks, the prose census tranches) would otherwise enumerate against an incomplete system list.
#### authority lines 3494-3497
03494:
03495: 1. Enumerate the sealed surface's vocabulary: the seal's forbidden content classes, from the seal record and its closeout, not from memory.
03496: 2. Write the guard test as a vocabulary assertion over the new tranche: no new entry may introduce, imply, or foreshadow the sealed class (signal scenarios for the distress seal).
03497: 3. Run the guard on every tranche of the genre, forever — the guard is permanent, not one-time.
… 26 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.

**Requested behavior.** Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.

**Minimum safe delta.** Extend `phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.

**Required delta.** Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.

**Primary seam.** phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `PhantomMemoryEngine`, `HeirloomSystem`, `ConfessionSecretSystem`, `MemorialSystem`, `phantom_heirlooms`, `provenance`.

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
| Domain rules | Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 21.

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
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.
- What is the smallest safe change? Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.
- Which owner is touched? Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/PhantomMemoryEngine.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Phantoms/HeirloomSystem.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Phantoms/HeirloomCatalog.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Phantoms/ConfessionSecretSystem.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Phantoms/ConfessionSecretCatalog.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/PhantomMemoryHostSession.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/PhantomMemorySaveStore.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.OrphanSealWave1.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/PhantomMemoryPanel.cs` — Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/phantom_triggers.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/phantom_heirlooms.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/confession_secrets.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/characters.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/survivors.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/PhantomMemoryPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/JournalPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/InventoryDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/EpiloguePanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/PhantomMemoryEngineTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/HeirloomSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections is wired end to end or the plan explicitly closes as already integrated.
- Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

PhantomMemoryEngine, PhantomTriggerDto, HeirloomSystem/Catalog, ConfessionSecretSystem/Catalog, MemorialSystem, PhantomMemoryHostSession, data catalogs, and focused tests exist. The old premise that no heirloom catalog exists is false; the plan should now target coverage, continuity, and host/save truth.

# 3. Required Delta

Connect object provenance, holder history, memory discovery, confession leverage, memorial transfer, and journal/audio facts while retaining each existing authority.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Plan 41 memorial/heirloom behavior and current confession host wiring are live; this plan extends their contracts without changing their semantics. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `childs_mitten`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `worn_photograph`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `undelivered_mail`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `radio_headset`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `battery`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `item_document_radio_log`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `miners_tag`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `item_signal_lamp_module`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `item_rock_salt_sack`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `book`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `item_archive_index_cylinder`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `family_photograph`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `item_collectible_prayer_beads`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `item_collectible_prayer_book`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `item_collectible_civic_token`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `item_dog_tags_scavenged`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `military_supply_crate`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `family_apartment_key`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `canned_food`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `recipe_card`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `recipe_tin`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `dog_tags`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `tarnished_medal`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `field_dressing_kit`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `engraved_lighter`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `stethoscope`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `nurse_fob_watch`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `pocket_notebook`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `childs_drawing`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `teachers_stamp`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `engineers_slide_rule`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `machinist_caliper`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `farm_ledger`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `tram_punch`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `mechanic_gloves`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `foreman_whistle`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `item_collectible_topo_map`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `item_theodolite_brass_precision`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `wooden_plank`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `chemical_solvent`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `medical_kit`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `item_document_triage_record`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `item_surgical_kit`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `item_collectible_road_map`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `fuel_canister`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `item_expedition_winch_kit`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `wedding_ring`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `bus_ticket`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `shopping_list`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `enamel_mug`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `cheap_comb`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `matchbook`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `creased_receipt`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `keyring_charm`
- Source: `Assets/StreamingAssets/Data/phantom_triggers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `item_decon_chelator_concentrate`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `item_lead_lined_effluent_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `item_heavy_neoprene_scrub_brush`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `item_sealed_waste_bin`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `item_surveyor_stadia_rod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `item_datum_plate_bronze`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `item_concrete_mix`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `item_forged_rotor_shaft`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `item_magnetic_bearing_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `item_high_vacuum_pump`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `item_containment_ring_steel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `item_reinforced_concrete_vault`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `item_seismic_damper_pad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `item_vacuum_pump_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `item_bearing_grease`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `item_rotor_balancing_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `item_portable_pid_detector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `item_detector_sensor_module`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `item_hermetic_sample_ampoule`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `item_hot_dust_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `item_sludge_cake`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `item_tailings_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `dosimeter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `geiger_counter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `iodine_pills`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `anti_rad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `gas_mask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `hazmat_suit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `water_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `air_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `clean_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `irradiated_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `cloth`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `scrap_metal`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `bandage`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `raw_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `cooked_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `dirty_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `morphine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `chelation_agent`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `potassium_iodide`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `calibration_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `tweezers`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `splint`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `antibiotics`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `jewelry`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `diamond`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `currency`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `mechanical_parts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `electronic_scrap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `item_radiosonde`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `solar_cell`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `chemicals`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `handheld_radio`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `engine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `roots`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `berries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `vacuum_tube`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `spring_mechanism`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `phonograph_needle`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `projector_bulb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `lubricant_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `film_reel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `antenna_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `soldering_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `music_box_comb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `spring_key`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `typewriter_ribbon`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `machine_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `camera_lens_cleaner`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `photographic_film`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `item_acoustic_decoy`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `item_ammonium_nitrate_sack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `item_amnestic_syrup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `item_anchor_notes`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `item_ash_ghillie`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `item_bio_plastic`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `item_black_water_vial`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `item_co2_scrubber_cartridge`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `item_epoxy_injector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `item_faraday_mesh`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `item_frostbite_salve`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `item_fungicide_fogger`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `item_galvanized_rebar`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `item_glycol_antifreeze_canister`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `item_hermetic_hatch_silicone_gasket`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `item_high_tensile_steel_culvert_brace`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `item_insulated_snowmobile_battery`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `item_lead_shielded_sample_cask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `item_lead_visor`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `item_lithium_salts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `item_mine_prod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `item_mycelium_bricks`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `item_prussian_blue_chelating_pellets`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `item_radon_detector_electret`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `item_rebreather_scrubber`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `item_ro_membrane`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `item_scopolamine_root`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `item_sealed_lead_pig`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `item_snow_goggles_improvised`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `item_sound_baffling`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `item_suitcase_locked`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `item_surgical_bone_chisel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `item_teddy_bear`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `item_thermal_paste`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `item_welders_glass`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `aa_batteries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `alcohol_wipes_box_10_of_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `ammo_762x54r_jhp_ap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `ammo_357`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `ammo_12g`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `ammo_308`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `ammo_556`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `ammo_762`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `antiseptic_1l_of_1l`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `battery_pack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `box_of_nails_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `canned_soup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `childrens_books`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `cigarette_lighter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `clean_water_jug`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `cooking_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `copper_wire_10m_of_10m`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `diesel_fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `dried_rations`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Resolve the record into a provenance dossier with holder, source, discovery gate, emotional fact, and exactly one owning state transition.
- Primary owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State/save rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI truth rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: memory, inheritance, confession, and provenance.
- Seam under test: phantom_triggers/phantom_heirlooms/confession_secrets -> PhantomMemoryEngine/HeirloomSystem/ConfessionSecretSystem -> memorial/lineage/journal/audio/UI projections.
- Expected authority: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache. Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI/accessibility check: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- Player-facing truth: Dead/missing holder, duplicate object, invalid trait, absent journal/audio, repeated confession, and old save without provenance preserve a safe unresolved state.
- Persistence response: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism response: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: PhantomMemoryEngine.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: HeirloomSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: ConfessionSecretSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: MemorialSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: phantom_heirlooms.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: provenance.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: PhantomMemoryEngine.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: HeirloomSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: ConfessionSecretSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: MemorialSystem.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: phantom_heirlooms.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: provenance.
- Owner: Each named system retains its own state; memorial/lineage own transfer and grief facts; host binds events without re-deciding them.
- State rule: Holder, provenance, discovered memory, confession choice, and transfer use existing heirloom/memorial/confession/phantom stores with explicit migration; no panel cache.
- Determinism rule: Trigger and memory selection use seeded campaign forks and stable object keys; transfer ordering is lineage/Ordinal stable.
- UI rule: ['src/UI/PhantomMemoryPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/InventoryDetailPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/PhantomMemoryEngineTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/HeirloomSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/HeirloomSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DwellerHeirloomCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/ConfessionSecretSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/Plan41MemoryActsTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Content/Plan41_45MemoryAcceptanceIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 479,360 characters.
# Post-250K deep polishing pass

The architecture body above reached 479,438 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `PhantomMemoryEngine`, `HeirloomSystem`, `ConfessionSecretSystem`, `MemorialSystem`, `phantom_heirlooms`, `provenance`.
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
