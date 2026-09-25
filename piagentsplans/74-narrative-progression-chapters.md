# Plan 74 — Narrative Progression Chapters: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** campaign progression, calendar, and epilogue
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Use the existing narrative progression loader, campaign calendar, and epilogue engine to make chapter progression a bounded projection of real milestones rather than a second campaign clock.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5756` characters.
- Current worktree copy: `493478` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `45723e1337e3711a7c42bf7b01daa0db01bbcbe752c1736273e21e213570bb2d`
- Snapshot size: 2033 characters; 59 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0024:     /// <summary>
0025:     /// Engine-agnostic loader for the 15 Narrative Progression Chapters.
0026:     /// Canonical data authority: Assets/StreamingAssets/Data/narrative_progression.json
0027:     /// </summary>
...
0053:                 // Malformed progression catalog: documented fallback to an empty
0054:                 // chapter list; the data-integrity gate owns authoring errors.
0055:                 return new List<NarrativeProgressionEntry>();
0056:             }
```

### Current evidence: `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c1ae619827695fb1fcce2a847d9b08f8e282e03746217511995f71b945ccf5d0`
- Snapshot size: 15399 characters; 399 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0015:     /// </summary>
0016:     public interface ICampaignCalendar
0017:     {
0018:         /// <summary>The authoritative campaign day (>= 1).</summary>
...
0030:         /// <summary>The current day's resolved calendar read model.</summary>
0031:         CampaignCalendarReadModel CurrentReadModel { get; }
0032:
0033:         /// <summary>Purely resolves time, seasonal context, and ambient baseline for any day without mutating state.</summary>
...
0048:
0049:     /// <summary>Default concrete implementation of <see cref="ICampaignCalendar"/>.</summary>
0050:     public sealed class CampaignCalendar : ICampaignCalendar
0051:     {
...
0058:
0059:         public CampaignCalendarReadModel CurrentReadModel => ResolveDay(_currentDay);
0060:
0061:         public event Action<int>? OnDayChanged;
...
0091:
0092:         public CampaignCalendarReadModel ResolveDay(int day)
0093:         {
0094:             if (day < 1) day = 1;
...
0097:             int year = ((day - 1) / DaysPerYear) + 1;
0098:             int chapter = year;
0099:
0100:             var seasons = _profile?.seasons;
...
0132:
0133:             return new CampaignCalendarReadModel(
0134:                 day: day,
0135:                 seasonId: currentWindow.id,
...
0141:                 year: year,
0142:                 chapter: chapter,
0143:                 ambientTemperatureC: ambientTemp,
0144:                 seasonalSeverity: severity,
...
0231:
0232:     /// <summary>Projects <see cref="ICampaignCalendar"/> to the historical <see cref="IClock"/> port.</summary>
0233:     public sealed class CalendarClockAdapter : IClock
0234:     {
...
0255:
0256:     /// <summary>Projects <see cref="ICampaignCalendar"/> to the <see cref="ISimClock"/> intraday clock.</summary>
0257:     public sealed class CalendarSimClockAdapter : ISimClock, IClock
0258:     {
...
0261:
0262:         private readonly ICampaignCalendar _calendar;
0263:         private long _intradayTicks;
0264:
...
```

### Current evidence: `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1d08733f013d54bbaaf966c282c46d47678bc7b95f46ac276fc3bceef7936400`
- Snapshot size: 2367 characters; 62 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009:     /// Single immutable domain snapshot for in-game time context:
0010:     /// day, season, seasonal progress, days-to-end, year, chapter, ambient baseline,
0011:     /// and seasonal modifiers. Engine-free.
0012:     /// </summary>
...
0022:         public int Year { get; } // 1-based
0023:         public int Chapter { get; } // 1-based
0024:         public float AmbientTemperatureC { get; }
0025:         public float SeasonalSeverity { get; } // 0.0 to 1.0
...
0029:
0030:         public CampaignCalendarReadModel(
0031:             int day,
0032:             string seasonId,
...
0038:             int year,
0039:             int chapter,
0040:             float ambientTemperatureC,
0041:             float seasonalSeverity,
...
0053:             Year = Math.Max(1, year);
0054:             Chapter = Math.Max(1, chapter);
0055:             AmbientTemperatureC = ambientTemperatureC;
0056:             SeasonalSeverity = Math.Clamp(seasonalSeverity, 0f, 1f);
```

### Current evidence: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c2d142ac86ed87cebfcc9c2c1d8bf85504859f923f3c124315df6862f92851f8`
- Snapshot size: 8617 characters; 220 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0028:     [Serializable]
0029:     public sealed class EpilogueChapter
0030:     {
0031:         public string Category { get; set; } = string.Empty;
...
0041:         public int TotalDays { get; set; }
0042:         public List<EpilogueChapter> Chapters { get; set; } = new List<EpilogueChapter>();
0043:         public CampaignEpilogueSnapshot FinalMetrics { get; set; } = new CampaignEpilogueSnapshot();
0044:
...
0060:
0061:             foreach (var chap in Chapters)
0062:             {
0063:                 sb.AppendLine($"--- {chap.Title} ({chap.Category}) ---");
...
0084:
0085:     public sealed class CampaignEpilogueEngine
0086:     {
0087:         private readonly CampaignEpilogueCatalog _catalog;
...
0113:             {
0114:                 chronicle.Chapters.Add(new EpilogueChapter
0115:                 {
0116:                     Category = "Demographics",
...
0134:             {
0135:                 chronicle.Chapters.Add(new EpilogueChapter
0136:                 {
0137:                     Category = "Governance",
...
0154:             {
0155:                 chronicle.Chapters.Add(new EpilogueChapter
0156:                 {
0157:                     Category = "Technology",
...
0174:             {
0175:                 chronicle.Chapters.Add(new EpilogueChapter
0176:                 {
0177:                     Category = "Sustenance",
```

### Current evidence: `Assets/Ashfall.Core/Campaign/CampaignEpilogueCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cc8229ed5d5f3b1796c149f26fffe6f7759511405cbabc11811e3e1702dc07e8`
- Snapshot size: 2592 characters; 73 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Text.Json;
0006:
0007: namespace Ashfall.Core.Campaign
0008: {
0009:     public sealed class EpilogueVignetteDef
0010:     {
0011:         public string id { get; set; } = string.Empty;
0012:         public string category { get; set; } = string.Empty;
0013:         public int priority { get; set; } = 1;
0014:         public int min_survivors { get; set; } = 0;
0015:         public int max_survivors { get; set; } = 999;
0016:         public int min_paroled_captives { get; set; } = 0;
0017:         public int max_penal_shifts { get; set; } = 999;
0018:         public int min_archives_decrypted { get; set; } = 0;
0019:         public int min_starvation_deaths { get; set; } = 0;
0020:         public int max_starvation_deaths { get; set; } = 999;
```

### Current evidence: `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `80b3302452337fc1724da033628414aaa6fc1c9909fe341c6539d89c6758f6b4`
- Snapshot size: 7354 characters; 150 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0029:     /// </summary>
0030:     public sealed class EpilogueMatrixRuntime
0031:     {
0032:         public RegionalFate EvaluateRegionalFate(EpilogueEvaluationContext ctx)
```

### Current evidence: `src/Host/ExpansionHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `9809fca6ed766dfd903f6dcc7df79e68f779d539a85bbd8a66066710d570da45`
- Snapshot size: 25659 characters; 500 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0034:         public GenerationalSuccessionEngine Generational { get; }
0035:         public EpilogueMatrixRuntime Epilogue { get; }
0036:         public DutyRosterSystem DutyRoster { get; private set; }
0037:         public Ashfall.Core.Foundry.SilentFoundrySystem SilentFoundry { get; private set; }
...
0074:             Generational = new GenerationalSuccessionEngine();
0075:             Epilogue = new EpilogueMatrixRuntime();
0076:             DutyRoster = new DutyRosterSystem();
0077:
...
0100:             Generational.OnTraitInherited += (_, _, _) => RaiseStateChanged();
0101:             Generational.OnChapterAdvanced += _ => RaiseStateChanged();
0102:         }
0103:
...
0472:             Generational.AdvanceTime(days);
0473:             return $"Advanced {days}d. Chapter {Generational.CurrentChapterIndex}, " +
0474:                    $"year {Generational.TotalYearsElapsed}.";
0475:         }
...
0486:             var save = Generational.CaptureState();
0487:             return $"Generational: ch {Generational.CurrentChapterIndex} · " +
0488:                    $"year {Generational.TotalYearsElapsed} · " +
0489:                    $"{save.generationRecords.Count} dwellers";
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

### Current evidence: `src/UI/ExpansionsHubPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/ExpansionsHubPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `01eff6eb5205c47ea31c84b2a3e55af72a649ad317844e2b4501e7456ec0d14d`
- Snapshot size: 22081 characters; 468 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0384:             var gen = _expansions?.Generational;
0385:             string genStatus = gen != null ? $"Chapter {gen.CurrentChapterIndex} · Year {gen.TotalYearsElapsed}" : "Active";
0386:             AddModuleCard(
0387:                 "EXP 12 — THE CENTURY SEED",
...
0392:                 {
0393:                     ("Succession Chapter", genStatus, CoreTheme.Warm),
0394:                     ("Mentoring Pairs", "Survivor trait inheritance active", CoreTheme.Pale),
0395:                     ("Legacy Score", "Multi-generational scorecards tracked", CoreTheme.Pale)
```

### Current evidence: `src/UI/JournalPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/JournalPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `412d1eb12aed0d996c58bb0b653d3b90517d40855ec738f9b5cfb4c139f5bf26`
- Snapshot size: 18870 characters; 486 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0312:             new AshfallSidebar.Item { Id = "manual",  Label = "Field Manual",  Hint = "SURVIVAL GUIDE" },
0313:         }, "CHAPTERS", "log");
0314:
0315:         _shell.AttachHeaderCloseButton("CLOSE [Esc]", () => OnClose?.Invoke());
```

### Current evidence: `Assets/StreamingAssets/Data/narrative_progression.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `f28901b0d212498d186dfa9ae9719ec743bf7c85f41979842bd0d815b499ff8c`
- Snapshot size: 3531 characters; 65 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0004:         {
0005:             "description": "Chapter 1 Complete: The Exchange — Nuclear detonations across the globe",
0006:             "order": 1
0007:         },
...
0012:         {
0013:             "description": "Chapter 3 Active: The Bunker — Establishing shelter and community",
0014:             "order": 3
0015:         },
...
0020:         {
0021:             "description": "Chapter 5 Pending: The Long Winter — Nuclear winter conditions setting in",
0022:             "order": 5
0023:         },
...
0028:         {
0029:             "description": "Chapter 7 Pending: The Long Dark — The cold becomes routine, which is worse than surprise. Work still gets done, but the shelter has begun measuring what it spends in sleep, patience, and people.",
0030:             "order": 7
0031:         },
...
0036:         {
0037:             "description": "Chapter 9 Pending: The Schism — The old agreements no longer hold everyone they were meant to hold. Factions split over what they can afford to become, and neutrality starts looking like a choice made for someone else.",
0038:             "order": 9
0039:         },
...
0044:         {
0045:             "description": "Chapter 11 Pending: The Reckoning — Nothing owed has disappeared just because it went uncollected. Old promises surface beside old grievances, and the shelter begins paying for decisions everyone hoped were finished.",
0046:             "order": 11
0047:         },
...
0052:         {
0053:             "description": "Chapter 13 Pending: The Second Winter — Winter returns to a world that has learned how to prepare and how to take. The shelter is harder to kill now; so are the people outside it.",
0054:             "order": 13
0055:         },
...
0060:         {
0061:             "description": "Chapter 15 Pending: The Inheritance — The question is no longer whether the shelter can endure another season. It is what will be handed forward, who will inherit it, and what they will be told it cost.",
0062:             "order": 15
0063:         }
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

### Current evidence: `Assets/StreamingAssets/Data/epilogue_personalization.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e912453e311f44e38a0362e7e75556582f2f310e4c67c2fe9bdd4c53d4cccc9e`
- Snapshot size: 5593 characters; 58 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "duration_variants": {
0004:     "short": "The brief experiment at the shelter lasted {days} days before the final reckoning arrived.",
0005:     "medium": "Across a grueling year of survival spanning {days} days, the shelter etched its enduring mark upon the wastes.",
0006:     "long": "Years stretched into legend over {days} days as the shelter outlasted storms, starvation, and the collapse of empires."
0007:   },
0008:   "political_templates": {
0009:     "military_schedule": "The Garrison's iron discipline became the bunker's law under the unrelenting schedule, establishing an unbending martial order.",
0010:     "military_default": "The Garrison incorporated the shelter into its forward defense perimeter, enforcing stability through military presence.",
0011:     "rebel_dark_road": "The resistance's fire spread from the wastes into the shelter along the dark road, tearing down old hierarchies.",
0012:     "rebel_default": "The free survivors formed a decentralized commonwealth, fiercely defending their autonomy from regional despots.",
0013:     "independent_tender": "The Fleet's arrival transformed the bunker from an isolated refuge into a thriving crossroads port for trade caravans.",
0014:     "independent_default": "Standing resolute without external masters, the shelter prospered through pragmatic alliances and fierce self-reliance.",
0015:     "prpf_reserve": "The hidden power emerged from the shadows to claim what was owed from the reserve, establishing covert governance.",
0016:     "prpf_default": "The clandestine PRPF network integrated the shelter's stockpiles into their shadow economy.",
0017:     "none_fractured": "Without outside alliances, the bunker stood alone against the wastes as fractured warlords squabbled in the distance."
0018:   },
0019:   "social_templates": {
0020:     "the_open_muster": "The substation rally point grew into an open commonwealth where demobilized conscripts and wanderers built anew.",
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

### Current evidence: `src/UI/EventsLogPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `16a057f71f9498ea2765815a98b1e0f41992ceb8fad96ff8ba95a059b8cda65d`
- Snapshot size: 7915 characters; 199 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Linq;
0004: #pragma warning disable CS8618
0005: using Godot;
0006: using Ashfall.Core.UI;
0007: using AtomicWar.GodotApp.UI;
0008: using AtomicWar.GodotApp.Host;
0009:
0010: namespace AtomicWar.GodotApp.UI
0011: {
0012:     /// <summary>
0013:     /// ASHFALL — Events Log panel.
0014:     /// Shows detailed event history, incident reports, and narrative progression.
0015:     /// </summary>
0016:     public partial class EventsLogPanel : Control
0017:     {
0018:         public event Action? OnClose;
0019:
0020:         private VBoxContainer _contentVBox = null!;
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
0014:     ///
0015:     /// Presentation only — evaluates EpilogueMatrixRuntime against simulation state.
0016:     /// </summary>
0017:     public partial class EpiloguePanel : Control
...
0020:
0021:         private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
0022:         private EpilogueEvaluationContext _context = new EpilogueEvaluationContext();
0023:         private CampaignOutcomeSnapshot? _snapshot;
```

### Current evidence: `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ae5d130bfda2baf18bda0c0dc4314dbd6edb0b7311c8f9e44f2455a45346fa0f`
- Snapshot size: 3917 characters; 99 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     /// <summary>
0013:     /// Plan 74 — Narrative Progression Chapters Expansion: 5 → 15 Campaign Chapters.
0014:     /// Verifies that narrative_progression.json loads 15 chapters with unique, contiguous ordering (1..15).
0015:     /// </summary>
...
0037:             var json = new SystemTextJsonSerializer();
0038:             return NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
0039:         }
0040:
...
0043:         {
0044:             var chapters = LoadCatalog();
0045:             Assert.Equal(15, chapters.Count);
0046:         }
...
0050:         {
0051:             var chapters = LoadCatalog();
0052:             var orders = chapters.Select(c => c.order).OrderBy(o => o).ToList();
0053:
...
0062:         {
0063:             var chapters = LoadCatalog();
0064:             foreach (var chapter in chapters)
0065:             {
...
0068:                 Assert.True(chapter.description.Length >= 20,
0069:                     $"Chapter order {chapter.order} description is too short ({chapter.description.Length} chars)");
0070:             }
0071:         }
...
0075:         {
0076:             var chapters = LoadCatalog();
0077:             var map = chapters.ToDictionary(c => c.order);
0078:
...
0085:
0086:             // Expanded chapters 6-15 present
0087:             Assert.Contains("The Consolidation", map[6].description);
0088:             Assert.Contains("The Long Dark", map[7].description);
```

### Current evidence: `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `29af66346147b510e997a3d544542a3455d1643dc83cd9a140bb4f1560c73339`
- Snapshot size: 6270 characters; 173 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009: {
0010:     public class CampaignCalendarTests
0011:     {
0012:         [Fact]
...
0042:         {
0043:             var cal = new CampaignCalendar(3);
0044:             var clock = cal.AsClock();
0045:
...
0059:         {
0060:             var cal = new CampaignCalendar(1);
0061:             var simClock = cal.AsSimClock();
0062:
...
0089:             };
0090:             var result1 = CampaignCalendarReconciler.Reconcile(consistent);
0091:             Assert.Equal(12, result1.AuthoritativeDay);
0092:             Assert.Equal("campaign_day", result1.PrimarySource);
...
0102:             };
0103:             var result2 = CampaignCalendarReconciler.Reconcile(drifted);
0104:             Assert.Equal(15, result2.AuthoritativeDay);
0105:             Assert.True(result2.HasMismatches);
...
0118:             };
0119:             var result3 = CampaignCalendarReconciler.Reconcile(legacy);
0120:             Assert.Equal(8, result3.AuthoritativeDay); // holdfast preferred over legacy
0121:             Assert.Equal("holdfast", result3.PrimarySource);
...
0126:         {
0127:             var calendar = new CampaignCalendar(1);
0128:             var coord = new CampaignDayCoordinator(calendar);
0129:
...
0152:         {
0153:             var cal = new CampaignCalendar(1);
0154:             var coord = new CampaignDayCoordinator(cal);
0155:             var clock = cal.AsClock();
```

### Current evidence: `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4884d624418aeb47023422eacd522a9aecbcd81cc76d47156db71d501b3072fc`
- Snapshot size: 4910 characters; 130 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009: {
0010:     public class CampaignCalendarPlan38Tests
0011:     {
0012:         [Fact]
...
0025:             Assert.Equal(1, model.Year);
0026:             Assert.Equal(1, model.Chapter);
0027:             Assert.True(model.AmbientTemperatureC >= -3.0f && model.AmbientTemperatureC <= 3.0f);
0028:             Assert.InRange(model.DayLengthHours, 6f, 16f);
...
0035:         {
0036:             var calendar = new CampaignCalendar(initialDay: 10);
0037:             Assert.Equal(10, calendar.CurrentDay);
0038:
...
0044:         [Fact]
0045:         public void MultiYear_CalculatesYearAndChapterMonotonically()
0046:         {
0047:             var calendar = new CampaignCalendar(initialDay: 1);
...
0050:             Assert.Equal(1, modelYear1.Year);
0051:             Assert.Equal(1, modelYear1.Chapter);
0052:
0053:             var modelYear1End = calendar.ResolveDay(365);
...
0058:             Assert.Equal(2, modelYear2Start.Year);
0059:             Assert.Equal(2, modelYear2Start.Chapter);
0060:             Assert.Equal("window_first_thaw", modelYear2Start.SeasonId);
0061:
...
0069:         {
0070:             var calendar = new CampaignCalendar(initialDay: 28);
0071:             var transitions = new List<(string oldSeason, string newSeason)>();
0072:
...
0095:         {
0096:             var calendar = new CampaignCalendar(initialDay: 180);
0097:
0098:             // At day 210, peak deep freeze reaches -45C
...
0119:
0120:             var calendar = new CampaignCalendar(initialDay: 1);
0121:             calendar.BindProfile(customProfile);
0122:
```

### Current evidence: `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a14260aa46c214df06369284f4724f0ab99750f1a070d555a1c33d73cade3e7e`
- Snapshot size: 7109 characters; 195 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public sealed class CampaignEpilogueEngineTests
0009:     {
0010:         private static CampaignEpilogueCatalog CreateMockCatalog()
...
0076:             var catalog = CreateMockCatalog();
0077:             var engine = new CampaignEpilogueEngine(catalog);
0078:
0079:             var snapshot = new CampaignEpilogueSnapshot
...
0096:             Assert.Equal(180, chronicle.TotalDays);
0097:             Assert.Equal(4, chronicle.Chapters.Count);
0098:
0099:             var demo = chronicle.Chapters.Find(c => c.Category == "Demographics");
...
0120:             var catalog = CreateMockCatalog();
0121:             var engine = new CampaignEpilogueEngine(catalog);
0122:
0123:             var snapshot = new CampaignEpilogueSnapshot
...
0136:
0137:             var demo = chronicle.Chapters.Find(c => c.Category == "Demographics");
0138:             Assert.NotNull(demo);
0139:             Assert.Equal("The Hollow Vault", demo.Title);
...
0145:             var catalog = CreateMockCatalog();
0146:             var engine = new CampaignEpilogueEngine(catalog);
0147:
0148:             var snap1 = new CampaignEpilogueSnapshot
...
0172:         [Fact]
0173:         public void FormattedReport_ContainsExpectedHeaderAndChapters()
0174:         {
0175:             var catalog = CreateMockCatalog();
```

### Current evidence: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `537c569811bfde768a3bf444a9baef9d8a8924a2e93baf06de5ae0618607c5e0`
- Snapshot size: 11288 characters; 258 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using System.IO;
0005: using System.Text.Json;
0006: using Xunit;
0007:
0008: namespace Ashfall.Core.Tests
0009: {
0010:     /// <summary>
0011:     /// Plan 104 — validates that narrative_questlines.json contains exactly 12 questlines,
0012:     /// all with unique quest_ids, all with 4 stages (Discovery/Investigation/Crisis/Resolution),
0013:     /// all Crisis stages having branch_a and branch_b, and all survivor_id and
0014:     /// target_location_id fields non-empty.
0015:     ///
0016:     /// This is a pure data-authority test: no new Core code is exercised, only the
0017:     /// JSON catalog's structural and referential integrity.
0018:     /// </summary>
0019:     public class NarrativeQuestlineCatalogTests
0020:     {
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/narrative_progression.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, entries`
- `entries`: list count=15; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `f28901b0d212498d186dfa9ae9719ec743bf7c85f41979842bd0d815b499ff8c`
#### `Assets/StreamingAssets/Data/campaign_epilogues.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, vignettes`
- `vignettes`: list count=9; sample IDs=['epilogue_demographics_thriving', 'epilogue_demographics_persevering', 'epilogue_demographics_desolation', 'epilogue_governance_reconciliation', 'epilogue_governance_iron_order', 'epilogue_technology_renaissance', 'epilogue_technology_makeshift', 'epilogue_sustenance_harvest']
- `schema_version`: `1`
- SHA-256: `8f645b326dde0e0f651aabd7a867cc88ea8e36157f292710117271d7524dca1f`
#### `Assets/StreamingAssets/Data/epilogue_personalization.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, duration_variants, political_templates, social_templates, moral_templates, survivor_fate_templates, discovery_templates, shelter_upgrade_templates`
- `schema_version`: `1`
- SHA-256: `e912453e311f44e38a0362e7e75556582f2f310e4c67c2fe9bdd4c53d4cccc9e`
#### `Assets/StreamingAssets/Data/quests_faction_branching.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, quests`
- `quests`: list count=200; sample IDs=['quest_bone_pickers_01', 'quest_bone_pickers_02', 'quest_bone_pickers_03', 'quest_bone_pickers_04', 'quest_bone_pickers_05', 'quest_bone_pickers_06', 'quest_bone_pickers_07', 'quest_bone_pickers_08']
- `schema_version`: `1`
- SHA-256: `e8eef1a2c1663bda30282be57f82178d830f1ea663cc173d97a99e54fe323786`
#### `Assets/StreamingAssets/Data/faction_war_events.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, chains`
- `chains`: list count=38; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `3ec09e02415a45ee4015120041651756e2cf7ada84fd70701abd5e289ef6e455`
## Symbol and caller audit

#### `NarrativeProgressionCatalogLoader` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=1, host=0, test=4
- `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs:28` (declaration) — public static class NarrativeProgressionCatalogLoader
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:38` (test) — return NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
- `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs:79` (test) — var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
- `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs:104` (test) — var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
- `Ashfall.Core.Tests/Campaign/Plan83_74WeatherNarrativeIntegrationTests.cs:133` (test) — var chapters = NarrativeProgressionCatalogLoader.Load(dataDir, io, json);
#### `CampaignCalendar` — HOST_REFERENCE_PRESENT — core/declaration=4, host=1, test=17
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:50` (declaration) — public sealed class CampaignCalendar : ICampaignCalendar
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:64` (core) — public CampaignCalendar(int initialDay = 1, Ashfall.Core.World.SeasonProfileDef? profile = null)
- `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs:54` (core) — Calendar = calendar ?? new CampaignCalendar(initialDay: 1);
- `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs:64` (core) — new CampaignCalendar(initialDay: 1),
- `src/Host/HostCli.WorldPlaytest.cs:408` (host) — new CampaignCalendar(1),
- `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs:15` (test) — var cal = new CampaignCalendar(initialDay: 1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs:43` (test) — var cal = new CampaignCalendar(3);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs:60` (test) — var cal = new CampaignCalendar(1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs:127` (test) — var calendar = new CampaignCalendar(1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs:153` (test) — var cal = new CampaignCalendar(1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:15` (test) — var calendar = new CampaignCalendar(initialDay: 1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:36` (test) — var calendar = new CampaignCalendar(initialDay: 10);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:47` (test) — var calendar = new CampaignCalendar(initialDay: 1);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:70` (test) — var calendar = new CampaignCalendar(initialDay: 28);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:96` (test) — var calendar = new CampaignCalendar(initialDay: 180);
- `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs:120` (test) — var calendar = new CampaignCalendar(initialDay: 1);
- `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs:104` (test) — var calendar = new CampaignCalendar(initialDay: 28);
- `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs:256` (test) — var calendar = new CampaignCalendar(initialDay: 100);
- `Ashfall.Core.Tests/Fixtures/CampaignFixture.cs:37` (test) — public CampaignCalendar Calendar { get; }
- `Ashfall.Core.Tests/Fixtures/CampaignFixture.cs:52` (test) — CampaignCalendar calendar,
- `Ashfall.Core.Tests/Fixtures/CampaignFixture.cs:87` (test) — var calendar = new CampaignCalendar(initialDay: 1);
- `Ashfall.Core.Tests/Fixtures/CampaignFixture.cs:122` (test) — var calendar = new CampaignCalendar(initialDay: 1);
#### `CampaignEpilogueEngine` — HOST_REFERENCE_PRESENT — core/declaration=4, host=3, test=7
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:756` (core) — ["campaign_epilogues.json"] = "CampaignEpilogueEngine",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1186` (core) — ["campaign_epilogues.json"] = new[] { "CampaignEpilogueEngine" },
- `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs:85` (declaration) — public sealed class CampaignEpilogueEngine
- `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs:89` (core) — public CampaignEpilogueEngine(CampaignEpilogueCatalog catalog)
- `src/Main.Plans62_65.cs:17` (host) — private CampaignEpilogueEngine? _epilogueEngine65;
- `src/Main.Plans62_65.cs:24` (host) — public CampaignEpilogueEngine? CampaignEpilogueEngine => _epilogueEngine65;
- `src/Main.Plans62_65.cs:94` (host) — _epilogueEngine65 = new CampaignEpilogueEngine(epilogueCatalog);
- `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs:77` (test) — var engine = new CampaignEpilogueEngine(catalog);
- `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs:121` (test) — var engine = new CampaignEpilogueEngine(catalog);
- `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs:146` (test) — var engine = new CampaignEpilogueEngine(catalog);
- `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs:176` (test) — var engine = new CampaignEpilogueEngine(catalog);
- `Ashfall.Core.Tests/Campaign/Plans62_65_SharedIntegrationTests.cs:148` (test) — var epilogueEng = new CampaignEpilogueEngine(epilogueCat);
- `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs:154` (test) — var epilogueEngine = new CampaignEpilogueEngine(epilogueCatalog);
- `Ashfall.Core.Tests/Campaign/Plan47_65ModAgencyIntegrationTests.cs:247` (test) — var epilogueEngine = new CampaignEpilogueEngine(epilogueCatalog);
#### `EpilogueMatrixRuntime` — HOST_REFERENCE_PRESENT — core/declaration=3, host=7, test=15
- `Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs:54` (core) — private static readonly EpilogueMatrixRuntime Runtime = new EpilogueMatrixRuntime();
- `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs:30` (declaration) — public sealed class EpilogueMatrixRuntime
- `Assets/Ashfall.Core/Verdict/VerdictEndingEvaluator.cs:10` (core) — /// scans. Mirrors EpilogueMatrixRuntime.EvaluateRegionalFate semantics so
- `src/Host/ExpansionHostSession.cs:35` (host) — public EpilogueMatrixRuntime Epilogue { get; }
- `src/Host/ExpansionHostSession.cs:75` (host) — Epilogue = new EpilogueMatrixRuntime();
- `src/Host/HostCli.PanelTests.cs:2274` (host) — /// EpilogueMatrixRuntime, DiveInstanceRunner) with functional checks and
- `src/Host/HostCli.PanelTests.cs:2377` (host) — // ── 4. EpilogueMatrixRuntime ────────────────────────────
- `src/Host/HostCli.PanelTests.cs:2378` (host) — var epilogue = new EpilogueMatrixRuntime();
- `src/UI/EpiloguePanel.cs:15` (host) — /// Presentation only — evaluates EpilogueMatrixRuntime against simulation state.
- `src/UI/EpiloguePanel.cs:21` (host) — private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:17` (test) — /// EpilogueMatrixRuntime, DiveInstanceRunner.
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:307` (test) — // ── EpilogueMatrixRuntime ───────────────────────────────────────────────
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:312` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:327` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:341` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:354` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:367` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:374` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:382` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:390` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs:398` (test) — var rt = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/Endgame/Plan19EndingContinuityTests.cs:17` (test) — private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/Endgame/Plan19SessionContinuityJourneyTests.cs:17` (test) — private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
- `Ashfall.Core.Tests/Medical/Plan16_19TriageEpilogueIntegrationTests.cs:126` (test) — var runtime = new EpilogueMatrixRuntime();
- … 1 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `narrative_progression` — HOST_REFERENCE_PRESENT — core/declaration=8, host=1, test=1
- `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs:26` (core) — /// Canonical data authority: Assets/StreamingAssets/Data/narrative_progression.json
- `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs:30` (core) — public const string DefaultFileName = "narrative_progression.json";
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:86` (core) — "narrative_progression.json", "narrative_questlines.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:496` (core) — ["narrative_progression.json"] = new[] { "NarrativeEncounterSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:825` (core) — ["narrative_progression.json"] = "NarrativeEncounterSystem",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1102` (core) — ["narrative_progression.json"] = new[] { "NarrativeEncounterSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1488` (core) — ["narrative_progression.json"] = new[] { "NarrativePanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1679` (core) — "narrative_encounters_expansion.json", "narrative_progression.json",
- `src/Host/EventsHostSession.cs:85` (host) — string narrativeJsonPath = CatalogPath.ResolveCatalog("narrative_progression.json");
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:14` (test) — /// Verifies that narrative_progression.json loads 15 chapters with unique, contiguous ordering (1..15).
#### `chapter` — HOST_REFERENCE_PRESENT — core/declaration=11, host=1, test=5
- `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:851` (core) — string chapter = NarrativeJsonHelpers.GetStringProp(sourceRecord, "synod_chapter");
- `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:856` (core) — subtitle = $"Chapter: {chapter} · Recorded: {timestamp}";
- `Assets/Ashfall.Core/Narrative/NarrativeDiscoveryCatalog.cs:860` (core) — defaultIdentity = "Unresolved institutional chapter; display only";
- `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs:54` (core) — // chapter list; the data-integrity gate owns authoring errors.
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:98` (core) — int chapter = year;
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs:142` (core) — chapter: chapter,
- `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs:10` (core) — /// day, season, seasonal progress, days-to-end, year, chapter, ambient baseline,
- `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs:39` (core) — int chapter,
- `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs:54` (core) — Chapter = Math.Max(1, chapter);
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs:31` (core) — /// Simulates 10x chapter timescale compression, elder retirement, mentor-apprentice skill transfer,
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs:36` (core) — public const int DaysPerChapter = 365; // 1 year per chapter
- `src/Host/HostCli.PanelTests.cs:2357` (host) — Check(gen.CurrentChapterIndex >= 1, "chapter index advanced or held");
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:64` (test) — foreach (var chapter in chapters)
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:66` (test) — Assert.False(string.IsNullOrWhiteSpace(chapter.description),
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:67` (test) — $"Chapter order {chapter.order} has empty description");
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:68` (test) — Assert.True(chapter.description.Length >= 20,
- `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs:69` (test) — $"Chapter order {chapter.order} description is too short ({chapter.description.Length} chars)");
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
#### authority lines 366-369
00366: ### Premise evidence
00367: VERIFIED: `moral_choice_flags.json`, `moral_choice_quests_distress.json`, `moral_choice_chains.json`, and the wider moral-choice catalog family exist live in the data authority. VERIFIED: v1.0 Part 5.6 documents the flags/ledger seam and the weight_of_choices epilogue codec (v2). VERIFIED (drift-corrected): the rescue-signal content wave added `moral_choice_quests_distress.json`, so the moral-choice loader family already consumes multiple split catalogs — the pattern for adding one more split catalog exists. HIGH CONFIDENCE: no current consumer re-reads door-choice flags after the near-term window (v1.0 Part 7 gap 2); the integration plan must re-grep flag consumers before implementation.
00368:
00369: ### Why this and not something else
#### authority lines 383-386
00383:
00384: ## Subject Plan F-002 — Mid-Winter Slump Pressure Campaign (Days 90–180)
00385:
00386: Lane A/C12 · Cluster C1, C7, C14 · Status PROPOSAL.
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
#### authority lines 453-456
00453: ### Continuity checklist result
00454: Culture-conditioned actions must not contradict doctrine behavior (doctrines remain the operational-behavior authority). Witness testimony must obey information-flow legality (a witness can only testify to what they experienced). Epilogue permutations touched must be declared per witness chain.
00455:
00456: ### Open premises
#### authority lines 466-469
00466: ### Premise evidence
00467: VERIFIED: the 19-wave closeouts in `INTEGRATION_PLANS.md` record the endgame work as complete with evidence (Endgame 84/84 PASS). VERIFIED: `epilogue_chronicle.json`, `campaign_epilogues.json`, `endings.json` exist live. HIGH CONFIDENCE: some permutations carry thinner chronicle prose than others (structural inference from any 32-cell matrix authored incrementally; the audit could not read per-permutation depth at listing level — verify in session).
00468:
00469: ### Why this and not something else
#### authority lines 679-682
00679:
00680: **A-07 · C3 · Root-cellar and silo follow-on field logs.** Subject: additional humidity-rot and weevil-audit entries conditioned on seasonal windows. Evidence: `root_cellar_humidity_rot_reports` and `grain_silo_weevil_audits` exist in the corpus; seasonal calendar is a canon system. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00681:
00682: **A-08 · C3 · Apiculture assay continuation.** Subject: Langstroth foundation-log continuation tied to seasonal yield and morale. Evidence: `langstroth_hive_foundation_logs` exists; apiculture is canon in Part 16.3. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 697-700
00697:
00698: **A-16 · C7 · Standing-record testimony depth.** Subject: witness-statement and registry-annotation prose expanding `standing_record_memory.json` coverage. Evidence: the standing-record family (factions, layouts, memory, quests) is verified live and is a canon epilogue evidence source. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00699:
00700: **A-17 · C7 · Verdict radio continuation.** Subject: verdict-station rundown batches conditioned on verdict questline state. Evidence: `verdict_radio.json` verified live; verdict questlines are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 721-724
00721:
00722: **A-28 · C13 · Under-served epilogue chronicle depth.** Subject: consumed by F-005 after the permutation audit selects the weakest cells. Evidence: matrix is canon (32 permutations). Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00723:
00724: **A-29 · C14 · Bestiary natural-history continuation.** Subject: sighting-log and specimen-record prose for bestiary entries with thin coverage. Evidence: `wasteland_wildlife_bestiary.json` verified live; vulture-sighting and cockroach-hive log genres exist. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 757-760
00757:
00758: **B-15 · C9 · Memorial-rite epilogue evidence enrollment.** Subject: performed rites enrolling as Reckoning evidence (rites exist; evidence vocabulary must be checked for a rite class before authoring). Evidence: `memorial_rites.json`, `spiritual_rituals.json` verified live. Route: CORE-EXTENSION through endgame owners. Confidence: PROPOSAL — evidence vocabulary check first.
00759:
00760: **B-16 · C10 · Quest reopening after new discoveries.** Subject: failed/abandoned quests reopening when discovery conditions later satisfy (the failure-recovery grammar of v1.0 Part 6.7). Evidence: abandoned-quest reopen is canon grammar; implementation state unverified. Route: CORE-EXTENSION through quest owners. Confidence: PROPOSAL.
… 60 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.

**Requested behavior.** Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.

**Minimum safe delta.** Extend `narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.

**Required delta.** Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.

**Primary seam.** narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `NarrativeProgressionCatalogLoader`, `CampaignCalendar`, `CampaignEpilogueEngine`, `EpilogueMatrixRuntime`, `narrative_progression`, `chapter`.

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
| Domain rules | Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
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

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 74.

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
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.
- What is the smallest safe change? Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.
- Which owner is touched? Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Narrative/NarrativeProgressionCatalogLoader.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Campaign/CampaignCalendarReadModel.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Campaign/CampaignEpilogueCatalog.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Endgame/EpilogueMatrixRuntime.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/ExpansionHostSession.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.GameFlow.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/ExpansionsHubPanel.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/JournalPanel.cs` — Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/narrative_progression.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/campaign_epilogues.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/epilogue_personalization.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/quests_faction_branching.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/faction_war_events.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/ExpansionsHubPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/JournalPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/EventsLogPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/EpiloguePanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections is wired end to end or the plan explicitly closes as already integrated.
- Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

NarrativeProgressionCatalogLoader, CampaignCalendar, CampaignEpilogueEngine/Catalog, EpilogueMatrixRuntime, narrative_progression.json, campaign epilogues, and focused tests already exist. The old plan’s claims about a 15-chapter enterprise engine and fresh pass claims must be rechecked against current loader and caller evidence.

# 3. Required Delta

Define chapter eligibility, one-shot milestone events, state-variant projections, calendar boundaries, ending inputs, save/restore, and truthful UI/journal/radio routing without changing the owners of unrelated systems.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Several progression-adjacent packages are active; the plan separates narrative coordination from ownership of quest, faction, weather, and ending state. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `epilogue_demographics_thriving`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `epilogue_demographics_persevering`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `epilogue_demographics_desolation`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `epilogue_governance_reconciliation`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `epilogue_governance_iron_order`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `epilogue_technology_renaissance`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `epilogue_technology_makeshift`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `epilogue_sustenance_harvest`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `epilogue_sustenance_famine`
- Source: `Assets/StreamingAssets/Data/campaign_epilogues.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `quest_bone_pickers_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `quest_bone_pickers_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `quest_bone_pickers_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `quest_bone_pickers_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `quest_bone_pickers_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `quest_bone_pickers_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `quest_bone_pickers_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `quest_bone_pickers_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `quest_bone_pickers_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `quest_bone_pickers_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `quest_blood_tithe_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `quest_blood_tithe_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `quest_blood_tithe_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `quest_blood_tithe_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `quest_blood_tithe_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `quest_blood_tithe_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `quest_blood_tithe_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `quest_blood_tithe_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `quest_blood_tithe_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `quest_blood_tithe_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `quest_drought_cartel_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `quest_drought_cartel_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `quest_drought_cartel_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `quest_drought_cartel_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `quest_drought_cartel_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `quest_drought_cartel_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `quest_drought_cartel_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `quest_drought_cartel_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `quest_drought_cartel_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `quest_drought_cartel_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `quest_martial_law_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `quest_martial_law_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `quest_martial_law_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `quest_martial_law_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `quest_martial_law_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `quest_martial_law_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `quest_martial_law_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `quest_martial_law_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `quest_martial_law_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `quest_martial_law_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `quest_guinea_pigs_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `quest_guinea_pigs_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `quest_guinea_pigs_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `quest_guinea_pigs_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `quest_guinea_pigs_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `quest_guinea_pigs_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `quest_guinea_pigs_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `quest_guinea_pigs_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `quest_guinea_pigs_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `quest_guinea_pigs_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `quest_piracy_mandate_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `quest_piracy_mandate_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `quest_piracy_mandate_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `quest_piracy_mandate_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `quest_piracy_mandate_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `quest_piracy_mandate_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `quest_piracy_mandate_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `quest_piracy_mandate_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `quest_piracy_mandate_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `quest_piracy_mandate_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `quest_iron_slaves_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `quest_iron_slaves_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `quest_iron_slaves_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `quest_iron_slaves_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `quest_iron_slaves_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `quest_iron_slaves_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `quest_iron_slaves_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `quest_iron_slaves_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `quest_iron_slaves_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `quest_iron_slaves_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `quest_quarantine_purge_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `quest_quarantine_purge_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `quest_quarantine_purge_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `quest_quarantine_purge_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `quest_quarantine_purge_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `quest_quarantine_purge_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `quest_quarantine_purge_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `quest_quarantine_purge_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `quest_quarantine_purge_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `quest_quarantine_purge_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `quest_assimilation_protocol_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `quest_assimilation_protocol_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `quest_assimilation_protocol_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `quest_assimilation_protocol_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `quest_assimilation_protocol_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `quest_assimilation_protocol_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `quest_assimilation_protocol_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `quest_assimilation_protocol_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `quest_assimilation_protocol_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `quest_assimilation_protocol_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `quest_storm_cult_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `quest_storm_cult_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `quest_storm_cult_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `quest_storm_cult_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `quest_storm_cult_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `quest_storm_cult_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `quest_storm_cult_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `quest_storm_cult_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `quest_storm_cult_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `quest_storm_cult_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `quest_scrap_network_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `quest_scrap_network_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `quest_scrap_network_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `quest_scrap_network_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `quest_scrap_network_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `quest_scrap_network_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `quest_scrap_network_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `quest_scrap_network_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `quest_scrap_network_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `quest_scrap_network_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `quest_broken_spears_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `quest_broken_spears_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `quest_broken_spears_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `quest_broken_spears_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `quest_broken_spears_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `quest_broken_spears_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `quest_broken_spears_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `quest_broken_spears_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `quest_broken_spears_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `quest_broken_spears_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `quest_free_wells_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `quest_free_wells_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `quest_free_wells_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `quest_free_wells_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `quest_free_wells_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `quest_free_wells_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `quest_free_wells_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `quest_free_wells_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `quest_free_wells_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `quest_free_wells_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `quest_the_defenders_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `quest_the_defenders_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `quest_the_defenders_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `quest_the_defenders_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `quest_the_defenders_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `quest_the_defenders_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `quest_the_defenders_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `quest_the_defenders_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `quest_the_defenders_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `quest_the_defenders_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `quest_cure_seekers_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `quest_cure_seekers_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `quest_cure_seekers_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `quest_cure_seekers_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `quest_cure_seekers_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `quest_cure_seekers_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `quest_cure_seekers_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `quest_cure_seekers_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `quest_cure_seekers_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `quest_cure_seekers_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `quest_rescue_armada_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `quest_rescue_armada_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `quest_rescue_armada_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `quest_rescue_armada_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `quest_rescue_armada_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `quest_rescue_armada_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `quest_rescue_armada_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `quest_rescue_armada_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `quest_rescue_armada_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `quest_rescue_armada_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `quest_plowshares_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `quest_plowshares_02`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `quest_plowshares_03`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `quest_plowshares_04`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `quest_plowshares_05`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `quest_plowshares_06`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `quest_plowshares_07`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `quest_plowshares_08`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `quest_plowshares_09`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `quest_plowshares_10`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `quest_hospital_ships_01`
- Source: `Assets/StreamingAssets/Data/quests_faction_branching.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: For each progression record, identify the real owner event or flag, its day boundary, its durable state, and the exact downstream projection that makes the chapter meaningful.
- Primary owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State/save rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI truth rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: campaign progression, calendar, and epilogue.
- Seam under test: narrative_progression.json -> NarrativeProgressionCatalogLoader -> existing campaign day/milestone event seam -> calendar/epilogue/journal/expansion projections.
- Expected authority: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger. Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI/accessibility check: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- Player-facing truth: Missing flags, out-of-order milestones, duplicate chapter IDs, old saves, ending after a failed campaign, and simultaneous day events must resolve deterministically and preserve the last valid state.
- Persistence response: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism response: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: NarrativeProgressionCatalogLoader.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: CampaignCalendar.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: CampaignEpilogueEngine.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: EpilogueMatrixRuntime.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: narrative_progression.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: chapter.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: NarrativeProgressionCatalogLoader.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: CampaignCalendar.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: CampaignEpilogueEngine.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: EpilogueMatrixRuntime.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: narrative_progression.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: chapter.
- Owner: Narrative progression and campaign calendar own progression facts; epilogue owner resolves endings; quest/faction/weather systems expose their own facts.
- State rule: Current chapter, start day, milestone facts, and completion history must use existing campaign/epilogue state. No new parallel progression ledger.
- Determinism rule: Calendar boundaries and milestone evaluation are day/event ordered; any authored random cadence uses the existing seeded stream and stable tie-breaking.
- UI rule: ['src/UI/ExpansionsHubPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/EventsLogPanel.cs', 'src/UI/EpiloguePanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/Plan74NarrativeProgressionExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignCalendarTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignCalendarPlan38Tests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignEpilogueEngineTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 483,227 characters.
# Post-250K deep polishing pass

The architecture body above reached 483,305 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `NarrativeProgressionCatalogLoader`, `CampaignCalendar`, `CampaignEpilogueEngine`, `EpilogueMatrixRuntime`, `narrative_progression`, `chapter`.
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
