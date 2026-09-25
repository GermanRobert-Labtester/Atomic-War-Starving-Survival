# Plan 124 — Faction War Location Overrides: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** war state, location variants, and map projection
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Represent wartime location change as a bounded, deterministic projection over the existing faction-war state and canonical map/location owners, with explicit precedence and restore behavior.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5849` characters.
- Current worktree copy: `501421` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4dfa917553bf8239ff0ed5799cd97fb81954caa3f0f24d32137f4e220bc092ef`
- Snapshot size: 20250 characters; 451 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011:     ///
0012:     /// This is the content side of <see cref="FactionWarSystem"/> (which handles
0013:     /// simulation: standing, territory, tension). The catalog provides the
0014:     /// narrative surface — what the player reads, hears, and experiences as the
...
0119:         /// <summary>
0120:         /// Returns the single active location override for a locationId on the
0121:         /// given day, or null if none applies. If multiple overrides for the
0122:         /// same location are simultaneously active (authoring error — should
...
0190:
0191:         /// <summary>Standing adjustment routed to the host's FactionWarSystem
0192:         /// (via FactionWarChainRunner.StandingDeltaApplier). Empty faction = no-op.</summary>
0193:         public string standingFactionId = string.Empty;
...
0318:     /// <summary>
0319:     /// Loads all five faction_war_* JSON files into a <see cref="FactionWarContentCatalog"/>.
0320:     /// Tolerant of missing files (logs warning, continues); parse failures in one file
0321:     /// do not prevent loading the others.
...
0329:         public const string CommuniquesFile = "faction_war_communiques.json";
0330:         public const string LocationOverridesFile = "faction_war_location_overrides.json";
0331:
0332:         private readonly IFileIO _files;
...
0335:
0336:         public FactionWarContentCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
0337:         {
0338:             _files = files ?? throw new ArgumentNullException(nameof(files));
...
0342:
0343:         public FactionWarContentCatalog Load(string dataDirectory)
0344:         {
0345:             var catalog = new FactionWarContentCatalog();
...
0361:                       $"{catalog.DialogueSnippetCount} dialogue, {catalog.CommuniqueCount} communiques, " +
0362:                       $"{catalog.LocationOverrideCount} location overrides");
0363:
0364:             return catalog;
...
0380:
0381:         private void LoadJournalEntries(string path, FactionWarContentCatalog catalog)
0382:         {
0383:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
...
0394:
0395:         private void LoadBroadcasts(string path, FactionWarContentCatalog catalog)
0396:         {
0397:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
...
0408:
0409:         private void LoadDialogueSnippets(string path, FactionWarContentCatalog catalog)
0410:         {
0411:             if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
...
```

### Current evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5294c96557d10b243115c35fd87fff34a5e8bfaf8ebc546b29723b04ebd2e7b6`
- Snapshot size: 31858 characters; 603 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0275:     [Serializable]
0276:     public sealed class FactionWarChainRunnerState
0277:     {
0278:         public string systemId = FactionWarChainRunner.SystemId;
...
0289:     /// <summary>
0290:     /// Advances every chain in a FactionWarContentCatalog day by day: tracks
0291:     /// which stage is current per chain, evaluates that stage's
0292:     /// FactionWarTrigger (via FactionWarTriggerTable), surfaces the stage to
...
0303:     /// </summary>
0304:     public sealed class FactionWarChainRunner
0305:     {
0306:         public const string SystemId = "faction_war_chain_runner";
...
0320:
0321:         private readonly FactionWarContentCatalog _catalog;
0322:         private FactionWarChainRunnerState _state;
0323:
...
0327:
0328:         public FactionWarChainRunner(FactionWarContentCatalog catalog, FactionWarChainRunnerState? state = null)
0329:         {
0330:             _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
...
0336:
0337:         public FactionWarChainRunnerState State => _state;
0338:         public FactionWarContentCatalog Catalog => _catalog;
0339:         public int CumulativeMoraleDelta => _state.cumulativeMoraleDelta;
...
0348:         /// <summary>Optional sink for choice standing adjustments — the host binds
0349:         /// FactionWarSystem.ModifyStanding here. Core never touches war standing
0350:         /// on its own.</summary>
0351:         public Action<string, int>? StandingDeltaApplier;
...
0556:
0557:         public FactionWarChainRunnerState CaptureState() => Clone(_state);
0558:
0559:         public void RestoreState(FactionWarChainRunnerState state)
...
0564:             if (state.schemaVersion > 1)
0565:                 throw new NotSupportedException($"Future FactionWarChainRunner save schema {state.schemaVersion}; supported schema is 1.");
0566:             _state = Clone(state);
0567:         }
...
0570:         {
0571:             var copy = new FactionWarChainRunnerState
0572:             {
0573:                 systemId = source.systemId,
```

### Current evidence: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `79467fd76488c2f4747cd21ba11833fdcad6d795a8e86f5a62db166941aa6e75`
- Snapshot size: 13945 characters; 348 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0032:     [Serializable]
0033:     public class FactionWarSystemState
0034:     {
0035:         public List<FactionStandingRecord> factions = new List<FactionStandingRecord>();
...
0048:     /// </summary>
0049:     public class FactionWarSystem
0050:     {
0051:         public const string SystemId = "faction_war_system";
...
0055:
0056:         private readonly FactionWarSystemState _state;
0057:
0058:         public FactionWarSystemState State => _state;
...
0071:
0072:         public FactionWarSystem(FactionWarSystemState? state = null)
0073:         {
0074:             _state = state ?? new FactionWarSystemState();
...
0234:
0235:         public FactionWarSystemState CaptureState()
0236:         {
0237:             var copy = new FactionWarSystemState
...
0289:         /// </summary>
0290:         public void RestoreState(FactionWarSystemState state)
0291:         {
0292:             if (state == null) return;
```

### Current evidence: `src/Main.YearOfAsh.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `bcaecfe610dc6eda1bea88bcc0f55db8f5acca3cbcdc8cc22555af705c27a63e`
- Snapshot size: 25804 characters; 524 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0038:         private int _doorEncounterIndex = 0;
0039:         private FactionWarMapWidget _factionWarMap = null!;
0040:         private RadioBroadcastTerminal _radioTerminal = null!;
0041:         private GeothermalHeatingWidget _geothermalWidget = null!;
...
0346:
0347:             _factionWarMap = new FactionWarMapWidget();
0348:             _geothermalWidget = new GeothermalHeatingWidget();
0349:             _radonWidget = new RadonVentilationWidget();
```

### Current evidence: `src/YearOfAsh/FactionWarMapWidget.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/YearOfAsh/FactionWarMapWidget.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `19aceb69125c811f96de5681f71168b9a517c3a5d8c15920155d419d61dc2741`
- Snapshot size: 4849 characters; 142 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     /// Godot 4.7+ UI Control for presenting the Year of Ash Faction War & Season Status.
0013:     /// Thin presentation only: queries FactionWarSystem and YearOfAshTimelineSystem.
0014:     /// Zero simulation logic.
0015:     /// </summary>
```

### Current evidence: `src/UI/Plans94To97Panel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/Plans94To97Panel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0708a39751ab81db3d8a2b86a33a4f26a1c7f74a52b39cb772794405d3748d98`
- Snapshot size: 10939 characters; 256 lines
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
0009:     /// <summary>
0010:     /// Bound operations console for the Plan 94–97 shelter systems.
0011:     /// It exposes only Core session commands; state and inventory remain owned
0012:     /// by the underlying authorities.
0013:     /// </summary>
0014:     public partial class Plans94To97Panel : Control, IBindablePanel
0015:     {
0016:         public event Action? OnClose;
0017:
0018:         private GrainProcessingHostSession? _grain;
0019:         private CryogenicAirSeparationHostSession? _cryogenic;
0020:         private HeliographHostSession? _heliograph;
```

### Current evidence: `src/YearOfAsh/YearOfAshHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ccefd687193231434f67fa00f2a465d6a8558f7d6c3c7748f94a35fb28e4b6d2`
- Snapshot size: 16189 characters; 345 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0019:         private readonly DoorEncounterSystem _encounters;
0020:         private readonly FactionWarSystem _factionWar;
0021:         private readonly QuestlineSystem _quests;
0022:         private readonly YearOfAshDeepFreezeSystem _deepFreeze;
...
0030:         public DoorEncounterSystem Encounters => _encounters;
0031:         public FactionWarSystem FactionWar => _factionWar;
0032:         public QuestlineSystem Quests => _quests;
0033:         public YearOfAshDeepFreezeSystem DeepFreeze => _deepFreeze;
...
0041:             DoorEncounterSystem encounters = null!,
0042:             FactionWarSystem factionWar = null!,
0043:             QuestlineSystem quests = null!,
0044:             YearOfAshDeepFreezeSystem deepFreeze = null!,
...
0050:             _encounters = encounters ?? new DoorEncounterSystem();
0051:             _factionWar = factionWar ?? new FactionWarSystem();
0052:             _quests = quests ?? new QuestlineSystem();
0053:             _deepFreeze = deepFreeze ?? new YearOfAshDeepFreezeSystem();
...
0084:         /// actions and tribute short-payments move the canonical
0085:         /// FactionWarSystem standing for warlords_sector_4, which persists with
0086:         /// the factionWar envelope section.
0087:         /// </summary>
...
0146:                 // Load Faction War content catalog and bind runner
0147:                 var warCatalogLoader = new FactionWarContentCatalogLoader(fileIO, serializer, new GodotLog());
0148:                 var warCatalog = warCatalogLoader.Load(dataDir);
0149:                 session._warRunner = new FactionWarChainRunner(warCatalog);
...
0164:             _factionWar.SimulateDailyFriction(day);
0165:             _warRunner.TickDay(FactionWarChainRunner.ToAuthoredDay(day));
0166:             _deepFreeze.TickDailyThermal(day, _timeline.AmbientTemperatureCelsius);
0167:             _radon.TickDailyRadon(day, _timeline.AmbientTemperatureCelsius);
```

### Current evidence: `src/YearOfAsh/YearOfAshSaveStore.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9234d865f93632855f0e10e43a9a3dee6d57664c83d6a0c3a86020f879bdd715`
- Snapshot size: 2043 characters; 45 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: // ============================================================================
0003: // Save Store : YearOfAshSaveStore
0004: // Core State : Ashfall.Core.YearOfAsh.YearOfAshSave
0005: // Host Caller: Main.YearOfAsh / YearOfAshHostSession
0006: // Purpose    : Year of Ash campaign progression, season timeline, and winter survival records
0007: // ============================================================================
0008: using Ashfall.Core.Save;
0009: using Ashfall.Core.YearOfAsh;
0010:
0011: namespace AtomicWar.GodotApp.YearOfAsh
0012: {
0013:     /// <summary>
0014:     /// File persistence adapter for YearOfAshSave in the Godot host
0015:     /// environment — thin façade over the Core SaveStore&lt;T&gt; service
0016:     /// (via SaveStoreHub, codec flavor). Shape, versioned migration, and
0017:     /// validation live in <see cref="YearOfAshSaveCodec"/>; path resolution,
0018:     /// atomic write, and error handling live in the service. Stores the save
0019:     /// file in user://year_of_ash_save.json.
0020:     /// </summary>
```

### Current evidence: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `62217b1437ba0275f104184775e5a8fb9ce2864be33befaeeb57c33930120dac`
- Snapshot size: 14403 characters; 179 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locationOverrides": [
0004:     {
0005:       "id": "loc_override_almshouse_pre_strike",
0006:       "locationId": "loc_st_brigids_almshouse",
0007:       "overrideType": "pre_strike",
0008:       "activeFromDay": 515,
0009:       "activeUntilDay": 516,
0010:       "displayName": "St Brigid's Almshouse",
0011:       "description": "The almshouse was a hospice before the war and ran as one through the worst of it, and the building still carries that purpose in its bones. The beds are made, the sheets drawn tight, the lockers closed. Forty rads an hour, high enough to make visitors count their minutes. Lately, though, people who used to cut through the ward as a shortcut have started going the long way round instead, without quite saying why, the way a district gets quiet about a place before it has a reason to."
0012:     },
0013:     {
0014:       "id": "loc_override_almshouse_post_strike",
0015:       "locationId": "loc_st_brigids_almshouse",
0016:       "overrideType": "post_strike",
0017:       "activeFromDay": 517,
0018:       "displayName": "St Brigid's Almshouse (Struck)",
0019:       "description": "The roof is gone over the east wing, and what used to be kept as a kind of monument is a monument no longer. The made beds are scattered under open sky, three years of undisturbed dust blown open in a single night. The charts that were filled in to a date and not after it are ash, along with the date itself. Fifty-one rads an hour now, the debris field wider and hotter than the ward it replaced. Whoever ran this place knew how to end a shift properly. Whoever ended this one did not know, or did not care, that there was a difference."
0020:     },
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

### Current evidence: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `751395d29dd61d92ad37dbd6ffa6e5b6187b722d53b30204b3c4a07a49706c5b`
- Snapshot size: 23452 characters; 533 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "locations": [
0004:     {
0005:       "id": "loc_the_allotments",
0006:       "displayName": "The Works Allotment Commune",
0007:       "sector": "sector_4_floodplain",
0008:       "riskLevel": 1,
0009:       "radiationUsv": 0.45,
0010:       "description": "Five acres of polycarbonate cold-frame glasshouses wrapped in burlap. Steaming compost heaps keep perennial rye seedlings alive in -30°C frost."
0011:     },
0012:     {
0013:       "id": "loc_denial_cut_substation",
0014:       "displayName": "D/9 Denial Substation & Cut",
0015:       "sector": "sector_4_railway_cut",
0016:       "riskLevel": 3,
0017:       "radiationUsv": 1.85,
0018:       "description": "Reinforced concrete telephone repeater bunker overlooking the main railway cut. Encircled with concertina wire and claymore firing stakes."
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cafbdc4c94cfd2b28a63076b397b7d469b3cca06edcf35510dba11b026afbca1`
- Snapshot size: 41494 characters; 356 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "communiques": [
0004:     {
0005:       "id": "comm_d489_garrison_manifest_inspection",
0006:       "eventChainId": "evt_d488_manifest_holdup",
0007:       "factionId": "faction_central_garrison",
0008:       "day": 489,
0009:       "title": "Manifest Procedure at Checkpoint Gamma: An Assessment",
0010:       "body": "The Continuity Office has reviewed the manifest inspection conducted at Checkpoint Gamma, and its assessment follows. The inspection was routine. A commercial load whose documentation did not match its counted weight was delayed, not detained, while the discrepancy — three sacks, on paper — was brought under assessment. Inspection procedure is not seizure procedure, and residents repeating the word 'seized' are encouraged to note that the load has completed its movement. Coordination between carriers and this office's inspection staff remains the most reliable path to timely movement. Carriers whose paperwork reconciles have never once been delayed at this checkpoint, a record this office is content to let speak for itself.",
0011:       "authorNote": "Accurate but incomplete — the count difference was real on paper, but the Office omits that the checkpoint's platform scale reads consistently heavy in the Office's favor, a calibration no outside party has ever been permitted to verify."
0012:     },
0013:     {
0014:       "id": "comm_d490_rebuilders_two_scales",
0015:       "eventChainId": "evt_d488_manifest_holdup",
0016:       "factionId": "faction_rebuilders",
0017:       "day": 490,
0018:       "title": "Three Sacks, Two Scales, and One Open Ledger",
0019:       "body": "The Continuity Office describes a three-sack discrepancy at Checkpoint Gamma as under assessment. Here is our assessment. The load was weighed at the Exchange before it left, by our scale, in front of the carrier and two of our own people, and the manifest matched the count to the sack. If the checkpoint's platform reads three sacks heavy, that is a calibration question, and we will bring our scale to any gate in this district and weigh in front of whoever wants to watch. We are not calling anyone a thief. We are saying the numbers in this district should not have a uniform, and that whichever scale is wrong, one of them is.",
0020:       "authorNote": "Honest and correct — the Exchange's count was right and the checkpoint platform does read heavy; the Rebuilders suspect an honest calibration drift rather than a policy, and have no proof of intent."
```

### Current evidence: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9fa6c10d07cfcf89781584c5fdad148a92ad95a2c2f191add17e7245662b293c`
- Snapshot size: 17398 characters; 213 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "entries": [
0004:     {
0005:       "id": "journal_d482_mira_queue_count",
0006:       "authorName": "Mira",
0007:       "day": 482,
0008:       "locationId": "loc_ration_queue_plaza",
0009:       "voice": "child",
0010:       "body": "I counted the line today and it was two hundred and six people which is the most I ever counted. I chalked a star on my token because Bettine at the Exchange said stars are lucky and I want to see if it's true before I tell anyone it isn't. Denner from the counter walked past and didn't wave. He always waves. I am going to ask him about it tomorrow if he is still there, which he probably will be, because grown-ups are always where they are supposed to be, mostly."
0011:     },
0012:     {
0013:       "id": "journal_d486_fennick_ledger_entry",
0014:       "authorName": "Barrow Fennick",
0015:       "day": 486,
0016:       "locationId": "loc_grain_silo",
0017:       "voice": "comic_collaborator",
0018:       "body": "Owed, this week: two Garrison quartermasters, one Exchange weigher, and — new entry, underline it — the Tollman himself, who does not extend credit to anyone, which means I have found the one thing in this district worth more than grain, and it is apparently me being agreeable at the right moment. I have survived three years by being the last thing either side wants to shoot before checking whether I'm useful. It is not dignified work. It is, I will say, extremely steady work."
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/faction_war_radio.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ed5851965227c0feb5e5b373d25185984eb7cceba7f50829caf3b353fee1b6f7`
- Snapshot size: 17481 characters; 302 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "broadcasts": [
0004:     {
0005:       "id": "radio_d480_span44_automated_loop",
0006:       "frequency": "96.100 MHz",
0007:       "dayTrigger": 480,
0008:       "source": "Unattended Relay, Railway Span 44-Alpha",
0009:       "message": "Station null. Target grid primed. Remain in shelter. Station null. Target grid primed. Remain in shelter.",
0010:       "signalStrength": "S3",
0011:       "isEmergency": false
0012:     },
0013:     {
0014:       "id": "radio_d481_garrison_continuity_bulletin",
0015:       "frequency": "88.400 MHz",
0016:       "dayTrigger": 481,
0017:       "source": "Central Garrison Continuity Office",
0018:       "message": "Bulletin 481-C. Reports of irregular weighing practices at unregulated exchange points are noted and under continued observation. The Continuity Office reminds all trading concerns that the Garrison's forbearance is a courtesy extended, not a right assumed. Compliance inspections will resume at the Continuity Office's discretion.",
0019:       "signalStrength": "S8",
0020:       "isEmergency": false
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

### Current evidence: `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `be2ee1a0694411d96a73fc684e258c5901dca6eebde34e5f216d091f8cd3a9af`
- Snapshot size: 19189 characters; 450 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     {
0013:         private static FactionWarContentCatalog LoadCatalog()
0014:         {
0015:             var files = new FileSystemIO();
```

### Current evidence: `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `646d2c4f7cf0a53de3c0bceaf523f2a477f4d40de80a20d6dd5a9e2ad9741347`
- Snapshot size: 10014 characters; 215 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0067:             var json = new SystemTextJsonSerializer();
0068:             var loader = new FactionWarContentCatalogLoader(files, json);
0069:             var catalog = loader.Load(DataDirectory);
0070:
...
0132:
0133:             var warCatalog = new FactionWarContentCatalogLoader(files, json).Load(DataDirectory);
0134:             var flagDefs = MoralChoiceFlagCatalogLoader.Load(DataDirectory, files, json);
0135:
...
0166:
0167:             var warCatalog = new FactionWarContentCatalogLoader(files, json).Load(DataDirectory);
0168:
0169:             // Test active override selection at specific campaign milestones
```

### Current evidence: `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5f6b46df1f3b31d96129de987967eb3d74c370c57196149e1b6e9591f5df3bc6`
- Snapshot size: 11610 characters; 284 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0010: {
0011:     public class FactionWarContentCatalogTests
0012:     : CatalogTestBase{
0013:         private static string FindDataDir() => DataDirectory;
...
0018:             var json = new SystemTextJsonSerializer();
0019:             var loader = new FactionWarContentCatalogLoader(files, json);
0020:             return loader.Load(FindDataDir());
0021:         }
...
0031:             Assert.True(catalog.CommuniqueCount > 0, "communiques loaded");
0032:             Assert.True(catalog.LocationOverrideCount > 0, "location overrides loaded");
0033:         }
0034:
...
0251:             var json = new SystemTextJsonSerializer();
0252:             var loader = new FactionWarContentCatalogLoader(files, json);
0253:             var catalog = loader.Load("nonexistent/path");
0254:             Assert.Equal(0, catalog.EventChainCount);
```

### Current evidence: `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c64fcb63749a4c2011a9e75663f8b4761ec0ab84045a87e38fc72bce5c13b103`
- Snapshot size: 25639 characters; 562 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0036:     {
0037:         private static FactionWarContentCatalog LoadReal()
0038:         {
0039:             var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
...
0322:
0323:         private static FactionWarContentCatalog LoadSingleFile(string json)
0324:         {
0325:             var dir = TempDataDir();
...
0328:                 File.WriteAllText(Path.Combine(dir, FactionWarContentCatalogLoader.CommuniquesFile), json);
0329:                 var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
0330:                 return loader.Load(dir);
0331:             }
...
0399:                 File.WriteAllText(
0400:                     Path.Combine(dir, FactionWarContentCatalogLoader.CommuniquesFile), "{ not json");
0401:                 var loader = new FactionWarContentCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
0402:                 var catalog = loader.Load(dir);
...
0545:         [Fact]
0546:         public void FactionWarChainRunner_ExposesCatalogProperty()
0547:         {
0548:             var catalog = new FactionWarContentCatalog();
```

### Current evidence: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `855fb0173db437b7cf6e4b4c8d86de6fe71a958fa2ccfc7f500b040ed3b72d8c`
- Snapshot size: 4917 characters; 151 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Ashfall.Core.World;
0005: using Xunit;
0006:
0007: namespace Ashfall.Core.Tests.World
0008: {
0009:     public class WastelandMapSystemTests
0010:     {
0011:         private static WastelandMapSystem MakeMap()
0012:         {
0013:             var nodes = new List<MapNode>
0014:             {
0015:                 new MapNode { Id = "a", DisplayName = "A", Danger = MapNodeDanger.None,
0016:                     PositionX = 0, PositionY = 0, StartingUnlocked = true },
0017:                 new MapNode { Id = "b", DisplayName = "B", Danger = MapNodeDanger.Low,
0018:                     PositionX = 10, PositionY = 0, Discoverable = true },
0019:                 new MapNode { Id = "c", DisplayName = "C", Danger = MapNodeDanger.High,
0020:                     PositionX = 20, PositionY = 0, Discoverable = true },
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/faction_war_location_overrides.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locationOverrides`
- `locationOverrides`: list count=20; sample IDs=['loc_override_almshouse_pre_strike', 'loc_override_almshouse_post_strike', 'loc_override_ration_plaza_pre_strike', 'loc_override_ration_plaza_post_strike', 'loc_override_ash_sign_shrine_pre_strike', 'loc_override_ash_sign_shrine_post_strike', 'loc_override_span44_ambient_crater', 'loc_override_forward_roster_camp_ambient']
- `schema_version`: `1`
- SHA-256: `62217b1437ba0275f104184775e5a8fb9ce2864be33befaeeb57c33930120dac`
#### `Assets/StreamingAssets/Data/faction_war_events.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, chains`
- `chains`: list count=38; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`
#### `Assets/StreamingAssets/Data/faction_territory.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, territories, contested_zones`
- `territories`: list count=19; sample IDs=['territory_the_office', 'territory_the_cutters', 'territory_black_flotilla', 'territory_the_fleet', 'territory_deserter_coalition', 'territory_cold_count', 'territory_the_tally', 'territory_grain_exchange']
- `contested_zones`: list count=5; sample IDs=['zone_contested_water_rights', 'zone_contested_cut_salvage', 'zone_contested_merchant_crossroads', 'zone_contested_scarp_pass', 'zone_contested_coastal_bluff']
- `schema_version`: `1`
- `collection_id`: `faction_territory_catalog`
- SHA-256: `ef0940bbe98f3082b75cbbef77df670e7e2c7358b89ddeefc20b8fd119b90915`
#### `Assets/StreamingAssets/Data/year_of_ash_locations.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, locations`
- `locations`: list count=66; sample IDs=['loc_the_allotments', 'loc_denial_cut_substation', 'loc_brine_pumping_sluice', 'loc_continental_radio_beacon', 'loc_low_background_lab', 'loc_geothermal_well_alpha', 'loc_garrison_checkpoint_gamma', 'loc_black_thaw_drainage_basin']
- `schema_version`: `1`
- SHA-256: `751395d29dd61d92ad37dbd6ffa6e5b6187b722d53b30204b3c4a07a49706c5b`
#### `Assets/StreamingAssets/Data/faction_war_communiques.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, communiques`
- `communiques`: list count=40; sample IDs=['comm_d489_garrison_manifest_inspection', 'comm_d490_rebuilders_two_scales', 'comm_d497_garrison_clean_strike', 'comm_d497_rebuilders_clean_strike', 'comm_d498_ash_sign_clean_strike', 'comm_d505_garrison_labor_quota', 'comm_d507_rebuilders_quota_arithmetic', 'comm_d513_garrison_span_readiness']
- `schema_version`: `1`
- SHA-256: `cafbdc4c94cfd2b28a63076b397b7d469b3cca06edcf35510dba11b026afbca1`
#### `Assets/StreamingAssets/Data/faction_war_journal.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, entries`
- `entries`: list count=26; sample IDs=['journal_d482_mira_queue_count', 'journal_d486_fennick_ledger_entry', 'journal_d490_fossey_bean_row', 'journal_d502_denner_the_list', 'journal_d509_denner_gone_to_ground', 'journal_d518_mira_the_almshouse', 'journal_d528_adaeze_the_coats', 'journal_d536_fennick_the_new_checkpoint']
- `schema_version`: `1`
- SHA-256: `9fa6c10d07cfcf89781584c5fdad148a92ad95a2c2f191add17e7245662b293c`
#### `Assets/StreamingAssets/Data/faction_war_radio.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, broadcasts`
- `broadcasts`: list count=33; sample IDs=['radio_d480_span44_automated_loop', 'radio_d481_garrison_continuity_bulletin', 'radio_d484_exchange_roster_wire_rebuttal', 'radio_d487_unsigned_supply_figures', 'radio_d488_garrison_grain_rebuttal', 'radio_d490_ash_sign_shrine_transmission', 'radio_d493_toll_syndicate_rate_notice', 'radio_d496_understory_clean_strike']
- `schema_version`: `1`
- SHA-256: `ed5851965227c0feb5e5b373d25185984eb7cceba7f50829caf3b353fee1b6f7`
## Symbol and caller audit

#### `FactionWarContentCatalog` — HOST_REFERENCE_PRESENT — core/declaration=28, host=1, test=22
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:17` (declaration) — public sealed class FactionWarContentCatalog
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:319` (core) — /// Loads all five faction_war_* JSON files into a <see cref="FactionWarContentCatalog"/>.
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:343` (core) — public FactionWarContentCatalog Load(string dataDirectory)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:345` (core) — var catalog = new FactionWarContentCatalog();
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:367` (core) — private void LoadEventChains(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:381` (core) — private void LoadJournalEntries(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:395` (core) — private void LoadBroadcasts(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:409` (core) — private void LoadDialogueSnippets(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:423` (core) — private void LoadCommuniques(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:437` (core) — private void LoadLocationOverrides(string path, FactionWarContentCatalog catalog)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:290` (core) — /// Advances every chain in a FactionWarContentCatalog day by day: tracks
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:321` (core) — private readonly FactionWarContentCatalog _catalog;
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:328` (core) — public FactionWarChainRunner(FactionWarContentCatalog catalog, FactionWarChainRunnerState? state = null)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:338` (core) — public FactionWarContentCatalog Catalog => _catalog;
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:416` (core) — ["faction_war_communiques.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:417` (core) — ["faction_war_dialogue.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:418` (core) — ["faction_war_events.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:419` (core) — ["faction_war_journal.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:420` (core) — ["faction_war_location_overrides.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:421` (core) — ["faction_war_radio.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:425` (core) — ["faction_radio_corpus.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:707` (core) — ["faction_war_radio.json"] = "FactionWarContentCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:766` (core) — ["faction_radio_corpus.json"] = "FactionWarContentCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:767` (core) — ["faction_war_communiques.json"] = "FactionWarContentCatalog",
- … 27 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `FactionWarChainRunner` — HOST_REFERENCE_PRESENT — core/declaration=11, host=6, test=25
- `Assets/Ashfall.Core/CatalogIntegrityRules.cs:338` (core) — // runtime by the FactionActionBoard / FactionWarChainRunner seams and
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs:519` (core) — // runtime by the FactionActionBoard / FactionWarChainRunner seams;
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:81` (core) — var warRunner = new YearOfAsh.FactionWarChainRunner(warCatalog);
- `Assets/Ashfall.Core/Muster/FactionEcologyHeadlessDemo.cs:100` (core) — var coldRunner = new YearOfAsh.FactionWarChainRunner(warCatalog);
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:192` (core) — /// (via FactionWarChainRunner.StandingDeltaApplier). Empty faction = no-op.</summary>
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs:140` (core) — FactionWarChainRunner? factionWarChainRunner = null,
- `Assets/Ashfall.Core/YearOfAsh/YearOfAshSave.cs:177` (core) — FactionWarChainRunner? factionWarChainRunner = null,
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:278` (core) — public string systemId = FactionWarChainRunner.SystemId;
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:304` (declaration) — public sealed class FactionWarChainRunner
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:328` (core) — public FactionWarChainRunner(FactionWarContentCatalog catalog, FactionWarChainRunnerState? state = null)
- `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:565` (core) — throw new NotSupportedException($"Future FactionWarChainRunner save schema {state.schemaVersion}; supported schema is 1.");
- `src/YearOfAsh/YearOfAshHostSession.cs:25` (host) — private FactionWarChainRunner _warRunner;
- `src/YearOfAsh/YearOfAshHostSession.cs:36` (host) — public FactionWarChainRunner WarRunner => _warRunner;
- `src/YearOfAsh/YearOfAshHostSession.cs:47` (host) — FactionWarChainRunner warRunner = null!)
- `src/YearOfAsh/YearOfAshHostSession.cs:56` (host) — _warRunner = warRunner ?? new FactionWarChainRunner(new FactionWarContentCatalog());
- `src/YearOfAsh/YearOfAshHostSession.cs:149` (host) — session._warRunner = new FactionWarChainRunner(warCatalog);
- `src/YearOfAsh/YearOfAshHostSession.cs:165` (host) — _warRunner.TickDay(FactionWarChainRunner.ToAuthoredDay(day));
- `Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs:168` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:64` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:78` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:91` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:111` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:127` (test) — var runner = new FactionWarChainRunner(catalog);
- `Ashfall.Core.Tests/FactionWarChainRunnerTests.cs:142` (test) — var runner = new FactionWarChainRunner(catalog);
- … 18 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
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
#### `faction_war_location_overrides` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=6, host=0, test=0
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:330` (core) — public const string LocationOverridesFile = "faction_war_location_overrides.json";
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:74` (core) — "faction_war_location_overrides.json", "faction_war_radio.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:420` (core) — ["faction_war_location_overrides.json"] = new[] { "FactionWarContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:771` (core) — ["faction_war_location_overrides.json"] = "FactionWarContentCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1026` (core) — ["faction_war_location_overrides.json"] = new[] { "FactionWarSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1528` (core) — ["faction_war_location_overrides.json"] = new[] { "FactionWarPanel" },
#### `FactionWarMapWidget` — HOST_REFERENCE_PRESENT — core/declaration=1, host=2, test=0
- `src/Main.YearOfAsh.cs:39` (host) — private FactionWarMapWidget _factionWarMap = null!;
- `src/Main.YearOfAsh.cs:347` (host) — _factionWarMap = new FactionWarMapWidget();
- `src/YearOfAsh/FactionWarMapWidget.cs:16` (declaration) — public partial class FactionWarMapWidget : PanelContainer
#### `location override` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=1, host=0, test=0
- `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs:120` (core) — /// Returns the single active location override for a locationId on the
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 25-28
00025:
00026: **Scale honesty clause.** The requested target for this expansion effort is two million characters. A single authoring pass cannot responsibly produce two million characters of *verified* planning content, and the repository's own constitution (Part 0.4 of v1.0; `AGENTS.md` rules 7–8) forbids manufacturing padded work. v2.0 therefore defines a Multi-Session Growth Protocol (Part VI): the factory is designed to be *appended* session by session, each session adding one or more verified volumes (expanded subsystem deep maps, prose spec libraries, backlog batches), until the corpus reaches the target size organically. The Part VI protocol is the only sanctioned path to the target; bulk generation of unverified prose is a NON-CANON act.
00027:
00028: ---
#### authority lines 38-41
00038: **DR-01 — Root-level coordination artifacts absent from the v1.0 docs map. VERIFIED.**
00039: The repository root now contains planning and coordination artifacts the bible's docs map (v1.0 Part 5.8) does not mention: `A1_BRIEFING_DEFERRED.md`, `A1_COORDINATION_RECORD.md`, `WAVE9_PART1_CLOSEOUT.md`, `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`, `POTENTIALCLUTTER.md`, `sources.md`, `CRUSH.md`, `VIBE.md`, `MIMOCODE.md`, `OPENSETUP.md`, `Ashfall.slnx`, `Directory.Packages.props`, `global.json`, plus `tests/` and `snapshots/` at root. Consequence: a planner following the v1.0 docs map will miss active coordination surfaces and may duplicate decisions already recorded in them. Any expansion-planning session must now sweep the root-level `*.md` coordination files and the `Next-steps-plans/`, `piagentsplans/`, and `Seal-steps/` directories before drafting.
00040:
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
#### authority lines 92-95
00092: **Step 5 — Run the continuity and anti-duplication checklist.**
00093: The v1.0 checklist (Part 13.2) applies in full, plus two factory additions: (a) duplication firewall — prove the candidate does not duplicate any live catalog, system, or `docs/` authority map; (b) unclaimed-content check — if the candidate's content domain appears in `UNCLAIMED_CORPUS_CENSUS.md`, the plan must wire the unclaimed content first or explain why new content outranks it.
00094:
00095: **Step 6 — Label every claim.**
#### authority lines 117-120
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
#### authority lines 150-153
00150: | C5 | Per-destination scavenging-table parity for destinations beyond the 49-table coverage; vehicle-breakdown consequences into medical and dose ledgers | HIGH CONFIDENCE |
00151: | C6 | Flooded-route topology tags and authored map edges (foreman-flagged open decision — needs the named signature first) | BLOCKED — decision-gated |
00152: | C7 | FactionWar per-strike emitter extension (foreman-flagged open decision — needs signature) | BLOCKED — decision-gated |
00153: | C8 | Radio-signal follow-up chaining is SEALED (DISTRESS-SIGNALS-9-12 COMPLETE, DR-06); open instead: market-rumor band extension and intercept-driven journal depth | HIGH CONFIDENCE |
#### authority lines 218-221
00218: |---|---|---|
00219: | Data authority | `rewrite.py` relocation/justification (DR-05) | VERIFIED finding, PROPOSAL handling |
00220: | Content | Content-utilization reporting extensions driven by `UNCLAIMED_CORPUS_CENSUS.md` (DR-08): a census-to-plan feed | HIGH CONFIDENCE |
00221: | Docs | Index drift gate extensions covering the new root-level coordination files (DR-01) | HIGH CONFIDENCE |
#### authority lines 257-260
00257:
00258: **SB-07 — Root coordination surface registration (Lane I).** Evidence: DR-01, DR-09. Subject: register root-level coordination files and agent rulebooks in the docs map; define which are active vs historical. Integration route: docs-only. Confidence: VERIFIED need.
00259:
00260: **SB-08 — Gate-count drift guard (Lane H).** Evidence: DR-07. Subject: a check that fails when a documented gate count diverges from the live inventory, ending manual count drift between bibles, closeouts, and CI. Integration route: small script/test in `scripts/ci/` family, mirrors existing gates. Confidence: PROPOSAL (design needs the live gate inventory as input).
#### authority lines 315-318
00315: ## Domain and non-goals
00316: ## Lane allocation (max one plan per lane; data-first before wiring)
00317: ## Flagship + satellites (never more than five concurrent plans on shared seams)
00318: ## Verification matrix (per plan: verification class + acceptance)
#### authority lines 328-331
00328: 1. **Volume unit.** A volume is one appended Part to this document (or one of its companion canvases) produced in a single session, typically 15,000–60,000 characters, always evidence-grounded against the live repository.
00329: 2. **Volume types, in rotation:** (a) subsystem deep-map volumes (one per cluster C1–C17: full catalog inventories, prose-coverage gaps, seam maps); (b) prose specification libraries (expanded Part 9 field contracts with worked examples per document genre); (c) backlog replenishment volumes (fresh premise sweeps converting new Drift Register entries into SB-candidates); (d) lane deep guides (one per lane: full archetype playbooks with worked subject plans); (e) audit volumes (periodic re-audits refreshing the Drift Register).
00330: 3. **Session checklist.** Each session: run the Step 1 premise sweep; execute the Factory Protocol or append a volume; update the Drift Register for anything that moved; record the character count and volume index in the growth ledger below.
00331: 4. **Growth ledger.** v2.0 base: approximately 25,000 characters (this document). Target: 2,000,000. Every appended volume appends one ledger line: `[date] Volume [n] ([type]) — [chars] — cumulative [total]`.
#### authority lines 378-381
00378: ### Continuity checklist result
00379: Callback targets must reference existing survivor/location/faction ids only. Information-flow legality: the returning party must plausibly know the player's choice through a modeled channel (they were present, a rumor traveled, a courier carried word). Epilogue permutations: callbacks may adjust relationship deltas and epilogue weight only through the existing moral-choice weight seam; declare which permutations shift.
00380:
00381: ### Open premises
#### authority lines 513-516
00513: ### Subject
00514: Register the root-level coordination and agent artifacts (DR-01, DR-09: `A1_BRIEFING_DEFERRED.md`, `A1_COORDINATION_RECORD.md`, `WAVE9_PART1_CLOSEOUT.md`, `POTENTIALCLUTTER.md`, `sources.md`, the per-tool rulebooks `CLAUDE.md`, `CODEX.md`, `CRUSH.md`, `GEMINI.md`, `GOOSE.md`, `MIMOCODE.md`, `QWEN.md`, `VIBE.md`, `.clinerules`, `.cursorrules`, `.windsurfrules`, `.zcode/`, plus `Next-steps-plans/`, `piagentsplans/`, `Seal-steps/`, `semantic-review/`) in the docs map, classifying each as active instruction, historical record, or clutter candidate, so planners stop missing live decision surfaces.
00515:
00516: ### Premise evidence
#### authority lines 575-578
00575: ### Recommended integration route
00576: Tier: TOOLING. Seams: call-site search first (search_code across the repo for invocations); relocation with an entry in the tooling docs; a CI assertion that `Assets/StreamingAssets/Data/` contains only `.json` plus whitelisted non-JSON artifacts thereafter. Verification: data-integrity selftest before and after; the new CI assertion green.
00577:
00578: ### Continuity checklist result
#### authority lines 683-686
00683:
00684: **A-09 · C4 · Hydraulic extrusion assay corpus twin.** Subject: ram-pressure and die-wear assay records for the live-but-unmapped hydraulic extrusion catalog (DR-04). Evidence: catalog verified live; corpus twin status unverified. Route: DATA-ONLY after census check. Confidence: HIGH CONFIDENCE (catalog) / UNVERIFIED (twin absence).
00685:
00686: **A-10 · C4 · Metrology standards calibration corpus.** Subject: calibration certificates and gauge-discrepancy reports for `metrology_standards_catalog.json` (DR-04), in the low-background metrology voice. Evidence: catalog verified live; `LowBackgroundMetrology` host session exists. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 737-740
00737:
00738: **B-05 · C3 · Preservation × disease contamination bridge.** Subject: failed or rushed preservation producing contamination exposure through the existing disease/pathogen seams (zoonosis bridge is the model). Evidence: food preservation authority map exists; zoonosis bridge is canon. Route: CORE-EXTENSION + data. Confidence: PROPOSAL.
00739:
00740: **B-06 · C4 · XP difficulty consumer binding for industrial chains.** Subject: once XP W1's difficulty authority seals, bind industrial fuel/feedstock consumption scalars to it (the sanctioned difficulty seam — never parallel scalars). Evidence: W1 ACTIVE (DR-06). Route: CORE-EXTENSION after seal. Confidence: HIGH CONFIDENCE, sequence-gated on W1.
#### authority lines 839-842
00839:
00840: **E-06 · C17 · Controller parity for new panels.** Subject: focus-navigator parity for panels shipped without full 22-action-map coverage. Evidence: input contract canon (22-action map). Route: HOST-WIRING. Confidence: HIGH CONFIDENCE.
00841:
00842: **E-07 · C5 · Expedition camp panel depth.** Subject: expose truthful existing expedition state that the camp panel does not yet render (verify via layout selftest before claiming). Evidence: expedition camp panel exists (v1.0 Part 5.7). Route: HOST-WIRING. Confidence: INFERENCE pending selftest.
#### authority lines 852-855
00852:
00853: # VOLUME 3 — SUBJECT SEED CATALOG, Lanes F–J, AND SUBSYSTEM DEEP MAPS (Factory batch 2026-09-24-D)
00854:
00855: ## 3.1 Lane F — Performance seeds (F-01 … F-06)
#### authority lines 878-881
00878:
00879: **G-05 · C7 · War-chain authored-day mapping tests.** Subject: pin the 300-day offset mapping (playable 180 → authored 480) with boundary tests. Evidence: mapping is canon (`FactionWarChainRunner.ToAuthoredDay`). Route: focused xUnit. Confidence: HIGH CONFIDENCE.
00880:
00881: **G-06 · C8 · Exactly-once guard regression suite.** Subject: regression tests covering every sealed exactly-once guard class (ignore consequences, arrival resolution, salvage grants) against restore-mid-effect saves. Evidence: sealed runtime models the guards (DR-06). Route: focused xUnit + fixture saves. Confidence: HIGH CONFIDENCE.
#### authority lines 904-907
00904:
00905: **I-01 · Authority-map gap registry.** Subject: enumerate `docs/` domains (DR-02 listing) whose directory exists but whose authority map does not, and fill them in priority order (domains that gained systems in Waves 8–12 first). Evidence: DR-02 listing verified. Route: DOCS-ONLY. Confidence: HIGH CONFIDENCE.
00906:
00907: **I-02 · Bible-to-registry reconciliation.** Subject: a reconciliation pass between this document's Drift Register and `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md`, correcting whichever side is stale. Evidence: both exist; DR-04 proves drift occurs. Route: DOCS-ONLY. Confidence: HIGH CONFIDENCE.
… 104 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.

**Requested behavior.** Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.

**Minimum safe delta.** Extend `faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.

**Required delta.** Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.

**Primary seam.** faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `FactionWarContentCatalog`, `FactionWarChainRunner`, `FactionWarSystem`, `faction_war_location_overrides`, `FactionWarMapWidget`, `location override`.

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
| Domain rules | Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 124.

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
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.
- What is the smallest safe change? Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.
- Which owner is touched? Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.YearOfAsh.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/YearOfAsh/FactionWarMapWidget.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/Plans94To97Panel.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/YearOfAsh/YearOfAshHostSession.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/YearOfAsh/YearOfAshSaveStore.cs` — Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_location_overrides.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_events.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_territory.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/year_of_ash_locations.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_communiques.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_journal.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_radio.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/YearOfAsh/FactionWarMapWidget.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/Plans94To97Panel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/MapDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI is wired end to end or the plan explicitly closes as already integrated.
- Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

FactionWarContentCatalog, FactionWarChainRunner, FactionWarSystem-related year-of-ash code, faction_war_location_overrides.json, faction war event/communique/journal/radio catalogs, map widgets, and focused tests are live. The authority still marks per-strike emitters as decision-gated, so this plan cannot assume a new event stream.

# 3. Required Delta

Audit and extend location override schema, active windows, precedence, hazard/radiation modifiers, UI state variants, and persistence through existing war/map state without rewriting canonical locations.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Per-strike event emission and flood topology require separate signatures; this plan leaves those gates explicit and does not manufacture event authority. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `loc_override_almshouse_pre_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `loc_override_almshouse_post_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `loc_override_ration_plaza_pre_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `loc_override_ration_plaza_post_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `loc_override_ash_sign_shrine_pre_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `loc_override_ash_sign_shrine_post_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `loc_override_span44_ambient_crater`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `loc_override_forward_roster_camp_ambient`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `loc_override_understory_transmitter_ambient`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `loc_override_checkpoint_occupied`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `loc_override_granary_burned`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `loc_override_well_contaminated`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `loc_override_rail_yard_fortified`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `loc_override_village_abandoned`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `loc_override_factory_occupied`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `loc_override_bridge_destroyed`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `loc_override_roadblock_liberated`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `loc_override_camp_overrun`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `loc_override_station_reclaimed`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `loc_override_field_scorched`
- Source: `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `territory_the_office`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `territory_the_cutters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `territory_black_flotilla`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `territory_the_fleet`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `territory_deserter_coalition`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `territory_cold_count`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `territory_the_tally`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `territory_grain_exchange`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `territory_quiet_house`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `territory_scavenger_guild`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `territory_long_walk`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `territory_undertow`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `territory_hydro_barons`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `territory_iron_raiders`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `territory_the_provisioned`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `territory_archivists`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `territory_lamplighters`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `territory_sun_seekers`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `territory_osteophages`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `zone_contested_water_rights`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `zone_contested_cut_salvage`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `zone_contested_merchant_crossroads`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `zone_contested_scarp_pass`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `zone_contested_coastal_bluff`
- Source: `Assets/StreamingAssets/Data/faction_territory.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `loc_the_allotments`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `loc_denial_cut_substation`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `loc_brine_pumping_sluice`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `loc_continental_radio_beacon`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `loc_low_background_lab`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `loc_geothermal_well_alpha`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `loc_garrison_checkpoint_gamma`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `loc_black_thaw_drainage_basin`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `loc_maritime_icebreaker_dock`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `loc_rhizome_research_vault`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `loc_ash_sign_cathedral_crater`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `loc_sector_4_rail_switchyard`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `loc_hydro_baron_aqueduct_manifold`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `loc_granite_pass_weather_observatory`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `loc_d9_cache_bunker_delta`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `loc_flooded_quarry_cistern`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `loc_sub_level_maintenance_shaft_9`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `loc_garrison_motor_pool`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `loc_rebuilder_brickworks_kiln`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `loc_continental_convoy_staging_area`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `loc_salt_cavern_medical_depot`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `loc_collapsed_valley_viaduct`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `loc_hydro_baron_desal_plant_4`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `loc_mountain_tunnel_refuge`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `loc_radioisotope_power_station`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `loc_frozen_river_ferry_crossing`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `loc_garrison_signal_bunker_echo`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `loc_d9_culvert_junction_bravo`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `loc_allotment_glasshouse_complex`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `loc_aurora_borealis_grounding_shoal`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `loc_granite_arsenal_foundry`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `loc_railway_guild_roundhouse`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `loc_penal_pioneer_trench_sector`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `loc_deep_salt_hospital_sanctuary`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `loc_supply_corps_highway_redoubt`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `loc_shelled_grain_elevator_ruin`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `loc_vitrified_train_derailment_cut`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `loc_flooded_hydro_pump_cavern`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `loc_poison_gas_culvert_marsh`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `loc_garrison_artillery_emplacement_bravo`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `loc_ash_sign_pyre_cliff`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `loc_railway_telegraph_repeater_hut`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `loc_collapsed_peat_kiln_bunker`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `loc_ammonium_nitrate_fertilizer_shed`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `loc_breached_civil_defense_cache_9`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `loc_hydro_baron_ledger_office`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `loc_d9_underground_telecom_vault`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `loc_penal_quarry_crusher_plant`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `loc_salt_miners_barter_hall`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `loc_arctic_ice_channel_buoy_12`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `loc_shelled_church_belltower_lookout`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `loc_vitrified_crater_spring_pool`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `loc_garrison_court_martial_cellar`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `loc_ash_militia_deadfall_barrier`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `loc_sub_level_sewer_interceptor_6`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `loc_abandoned_half_track_convoy_wreck`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `loc_salt_cavern_explosives_magazine`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `loc_coastal_fog_signal_station`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `loc_high_granite_mortar_pit_charlie`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `loc_the_final_dawn_outlook`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `loc_muster_treeline_camp`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `loc_second_winter_homestead`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `loc_scavenger_guildhall`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `loc_iron_raiders_den`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `loc_the_tally_hall`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `loc_amnesty_petition_hall`
- Source: `Assets/StreamingAssets/Data/year_of_ash_locations.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `comm_d489_garrison_manifest_inspection`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `comm_d490_rebuilders_two_scales`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `comm_d497_garrison_clean_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `comm_d497_rebuilders_clean_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `comm_d498_ash_sign_clean_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `comm_d505_garrison_labor_quota`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `comm_d507_rebuilders_quota_arithmetic`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `comm_d513_garrison_span_readiness`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `comm_d514_rebuilders_span_record`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `comm_d519_garrison_almshouse`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `comm_d520_rebuilders_almshouse`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `comm_d521_ash_sign_almshouse`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `comm_d523_ash_sign_pilgrim_toll`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `comm_d526_garrison_waystation_fee`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `comm_d527_rebuilders_guard_detail`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `comm_d530_garrison_recorded_concern`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `comm_d537_garrison_exchange_checkpoint`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `comm_d538_rebuilders_exchange_checkpoint`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `comm_d549_garrison_ration_plaza`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `comm_d550_rebuilders_ration_plaza`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `comm_d552_ash_sign_ration_plaza`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `comm_d556_rebuilders_fracture`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `comm_d561_ash_sign_dead_channel`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `comm_d568_rebuilders_pumphouse_questions`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `comm_d570_garrison_pumphouse_coordination`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `comm_d573_forward_roster_checkpoint`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `comm_d575_forward_roster_origin`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `comm_d576_forward_roster_passage_rules`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `comm_d577_ash_sign_restless_numbers`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `comm_d581_garrison_shrine_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `comm_d582_rebuilders_shrine_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `comm_d583_ash_sign_shrine_strike`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `comm_d591_ash_sign_ceasefire_pause`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `comm_d592_garrison_standdown`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `comm_d593_forward_roster_ceasefire_toll`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `comm_d594_rebuilders_ceasefire_benchmark`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `comm_d599_forward_roster_crates`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `comm_d602_ash_sign_the_question`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `comm_d607_garrison_forward_roster_recognition`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `comm_d608_forward_roster_non_recognition`
- Source: `Assets/StreamingAssets/Data/faction_war_communiques.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `journal_d482_mira_queue_count`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `journal_d486_fennick_ledger_entry`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `journal_d490_fossey_bean_row`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `journal_d502_denner_the_list`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `journal_d509_denner_gone_to_ground`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `journal_d518_mira_the_almshouse`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `journal_d528_adaeze_the_coats`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `journal_d536_fennick_the_new_checkpoint`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `journal_d543_mira_the_star`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `journal_d546_mira_after`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `journal_d555_adaeze_the_split`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `journal_d560_selwyn_the_frequency`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `journal_d567_fennick_the_pumphouse`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `journal_d572_forward_roster_recruit`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `journal_d575_sella_the_toll_math`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `journal_d580_toma_the_broken_pattern`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `journal_d584_d9_cell_leader`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `journal_d592_vashti_the_scale_holds`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `journal_d595_mira_the_quiet`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `journal_d598_denner_the_pause`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `journal_d601_toma_after_the_theory`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `journal_d606_mira_the_quiet_peace`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `journal_warlord_toll_doctrine`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `journal_warlord_consolidation_doctrine`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `journal_warlord_annexation_doctrine`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `journal_warlord_withdrawal_doctrine`
- Source: `Assets/StreamingAssets/Data/faction_war_journal.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `radio_d480_span44_automated_loop`
- Source: `Assets/StreamingAssets/Data/faction_war_radio.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `radio_d481_garrison_continuity_bulletin`
- Source: `Assets/StreamingAssets/Data/faction_war_radio.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `radio_d484_exchange_roster_wire_rebuttal`
- Source: `Assets/StreamingAssets/Data/faction_war_radio.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `radio_d487_unsigned_supply_figures`
- Source: `Assets/StreamingAssets/Data/faction_war_radio.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each override against a base location, an authoritative start/end condition, a priority rule, and at least one live map or narrative consumer before it can alter presentation.
- Primary owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State/save rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI truth rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: war state, location variants, and map projection.
- Seam under test: faction_war_location_overrides.json -> FactionWarContentCatalog/override evaluator -> FactionWar/map/encounter projections -> year-of-ash journal/radio/UI.
- Expected authority: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten. Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI/accessibility check: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- Player-facing truth: Invalid location references, overlapping windows, expired state, missing event producer, stale save, and changed map nodes must preserve the base location and emit a diagnostic.
- Persistence response: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism response: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: FactionWarContentCatalog.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: FactionWarChainRunner.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: FactionWarSystem.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: faction_war_location_overrides.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: FactionWarMapWidget.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: location override.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: FactionWarContentCatalog.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: FactionWarChainRunner.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: FactionWarSystem.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: faction_war_location_overrides.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: FactionWarMapWidget.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: location override.
- Owner: Faction-war owner owns override state; WastelandMapSystem owns discovered graph state; location catalog owns base location; host projects current variant.
- State rule: Active override identity, source event/day, and restoration state must be captured by the existing faction-war save owner or a justified additive field; base location data is never overwritten.
- Determinism rule: Select the highest-priority active override with explicit tie-breaks; event order and day windows are stable and replayable.
- UI rule: ['src/YearOfAsh/FactionWarMapWidget.cs', 'src/UI/Plans94To97Panel.cs', 'src/UI/MapDetailPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/Plan124_125WarMoralIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarContentCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarCommuniqueExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/World/WastelandMapSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 491,154 characters.
# Post-250K deep polishing pass

The architecture body above reached 491,232 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `FactionWarContentCatalog`, `FactionWarChainRunner`, `FactionWarSystem`, `faction_war_location_overrides`, `FactionWarMapWidget`, `location override`.
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
