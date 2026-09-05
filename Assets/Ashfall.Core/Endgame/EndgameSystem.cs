// SPDX-License-Identifier: MIT
// ASHFALL campaign endgame & epilogue authority (Plan 84 / Task B25).

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Endgame
{
    public enum EndgamePhase
    {
        Active = 0,
        Triggered = 1,
        Epilogue = 2,
        Sealed = 3
    }

    [Serializable]
    public sealed class EndingDef
    {
        public string id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string tone { get; set; } = string.Empty;
        public int min_days_survived { get; set; }
        public int min_living_survivors { get; set; }
        public string summary { get; set; } = string.Empty;
        public string epilogue_text { get; set; } = string.Empty;
        public string factions_alignment { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class EndingsCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<EndingDef> endings { get; set; } = new();
    }

    [Serializable]
    public sealed class CampaignEpilogueReport
    {
        public string endingId { get; set; } = string.Empty;
        public string endingTitle { get; set; } = string.Empty;
        public string tone { get; set; } = string.Empty;
        public string mainEpilogueProse { get; set; } = string.Empty;
        public List<string> memorialTributes { get; set; } = new();
        public List<string> factionReactions { get; set; } = new();
        public int daysSurvived { get; set; }
        public int livingSurvivors { get; set; }
        public int deceasedSurvivors { get; set; }
        public float finalMoraleAverage { get; set; }
        public int expeditionsCompleted { get; set; }
        public int sealedDay { get; set; }
    }

    [Serializable]
    public sealed class EndgameSaveState
    {
        public int schema_version { get; set; } = 1;
        public string systemId { get; set; } = EndgameSystem.SystemId;
        public EndgamePhase phase { get; set; } = EndgamePhase.Active;
        public string selectedEndingId { get; set; } = string.Empty;
        public CampaignEpilogueReport? epilogueReport { get; set; }
        public bool isSealed { get; set; }
    }

    public sealed class CampaignEvaluationContext
    {
        public int CurrentDay { get; set; }
        public int LivingSurvivors { get; set; }
        public int DeceasedSurvivors { get; set; }
        public float AverageMorale { get; set; }
        public int ExpeditionsCount { get; set; }
        public string DominantFaction { get; set; } = "independent";
        public bool ForceExtinction { get; set; }
        public bool ForceWinterFailure { get; set; }
        public bool TruthBroadcasted { get; set; }
        public bool VassalageAccepted { get; set; }
        public List<string> NotableFallenNames { get; set; } = new();
    }

    public sealed class EndgameSystem
    {
        public const string SystemId = "endgame";

        private readonly Dictionary<string, EndingDef> _catalog = new(StringComparer.Ordinal);
        private EndgameSaveState _state = new();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public event Action<EndingDef, CampaignEpilogueReport>? OnEndingTriggered;
        public event Action<CampaignEpilogueReport>? OnCampaignSealed;

        public EndgameSaveState State => _state;
        public IReadOnlyDictionary<string, EndingDef> Catalog => _catalog;
        public EndgamePhase Phase => _state.phase;
        public bool IsSealed => _state.isSealed;

        public EndgameSystem(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(84);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json)) return;
            var data = serializer.Deserialize<EndingsCatalogData>(json);
            if (data?.endings == null) return;
            _catalog.Clear();
            foreach (var e in data.endings)
            {
                if (!string.IsNullOrEmpty(e.id))
                    _catalog[e.id] = e;
            }
        }

        public EndingDef EvaluateEnding(CampaignEvaluationContext ctx)
        {
            if (ctx.LivingSurvivors <= 0 || ctx.ForceExtinction)
            {
                return GetEndingOrDefault("ending_silent_tombs");
            }
            if (ctx.ForceWinterFailure)
            {
                return GetEndingOrDefault("ending_frozen_silence");
            }
            if (ctx.VassalageAccepted)
            {
                return GetEndingOrDefault("ending_warlord_tribute");
            }
            if (ctx.TruthBroadcasted)
            {
                return GetEndingOrDefault("ending_the_reckoning");
            }
            if (string.Equals(ctx.DominantFaction, "garrison", StringComparison.OrdinalIgnoreCase))
            {
                return GetEndingOrDefault("ending_iron_hegemony");
            }
            if (string.Equals(ctx.DominantFaction, "black_flotilla", StringComparison.OrdinalIgnoreCase))
            {
                return GetEndingOrDefault("ending_exodus_to_sea");
            }
            if (string.Equals(ctx.DominantFaction, "humanitarian", StringComparison.OrdinalIgnoreCase) ||
                (ctx.LivingSurvivors >= 15 && ctx.AverageMorale >= 75f))
            {
                return GetEndingOrDefault("ending_wasteland_sanctuary");
            }
            if (ctx.CurrentDay >= 360)
            {
                return GetEndingOrDefault("ending_dawn_of_thaw");
            }

            // Fallback: search catalog for highest minimum day qualification
            EndingDef best = null!;
            foreach (var candidate in _catalog.Values)
            {
                if (ctx.CurrentDay >= candidate.min_days_survived &&
                    ctx.LivingSurvivors >= candidate.min_living_survivors)
                {
                    if (best == null || candidate.min_days_survived > best.min_days_survived)
                        best = candidate;
                }
            }
            return best ?? GetEndingOrDefault("ending_dawn_of_thaw");
        }

        public CampaignEpilogueReport GenerateEpilogue(EndingDef ending, CampaignEvaluationContext ctx)
        {
            var report = new CampaignEpilogueReport
            {
                endingId = ending.id,
                endingTitle = ending.title,
                tone = ending.tone,
                mainEpilogueProse = ending.epilogue_text,
                daysSurvived = ctx.CurrentDay,
                livingSurvivors = ctx.LivingSurvivors,
                deceasedSurvivors = ctx.DeceasedSurvivors,
                finalMoraleAverage = ctx.AverageMorale,
                expeditionsCompleted = ctx.ExpeditionsCount,
                sealedDay = ctx.CurrentDay
            };

            // Generate memorial tributes
            if (ctx.DeceasedSurvivors > 0)
            {
                if (ctx.NotableFallenNames.Count > 0)
                {
                    string names = string.Join(", ", ctx.NotableFallenNames);
                    report.memorialTributes.Add($"The memorial plaques of {names} remain etched into the shelter wall, honored by all who survived.");
                }
                else
                {
                    report.memorialTributes.Add($"{ctx.DeceasedSurvivors} comrades fell to the perils of radiation and cold, their sacrifices remembered in the morning muster.");
                }
            }
            else
            {
                report.memorialTributes.Add("Miraculously, not a single life was lost to the cold, a testament to disciplined rationing and tireless medical care.");
            }

            // Generate faction legacy reaction
            if (string.Equals(ending.factions_alignment, "garrison", StringComparison.OrdinalIgnoreCase))
            {
                report.factionReactions.Add("The Regional Garrison counts the shelter as its most formidable forward outpost in Sector 7.");
            }
            else if (string.Equals(ending.factions_alignment, "black_flotilla", StringComparison.OrdinalIgnoreCase))
            {
                report.factionReactions.Add("The Black Flotilla's admiralty logs record the bunker crew as honorary submariners and navigators.");
            }
            else if (string.Equals(ending.factions_alignment, "humanitarian", StringComparison.OrdinalIgnoreCase))
            {
                report.factionReactions.Add("Wasteland caravans carry tales of the open gates, directing desperate families across three territories toward the valley.");
            }
            else
            {
                report.factionReactions.Add("Independent and unbroken, the holdfast carved its own destiny without bowing to external masters.");
            }

            return report;
        }

        public bool TriggerEnding(CampaignEvaluationContext ctx)
        {
            if (_state.isSealed) return false;

            var ending = EvaluateEnding(ctx);
            var report = GenerateEpilogue(ending, ctx);

            _state.phase = EndgamePhase.Epilogue;
            _state.selectedEndingId = ending.id;
            _state.epilogueReport = report;

            OnEndingTriggered?.Invoke(ending, report);
            return true;
        }

        public bool SealCampaign(int day)
        {
            if (_state.isSealed) return false;
            if (_state.phase != EndgamePhase.Epilogue) return false;

            _state.phase = EndgamePhase.Sealed;
            _state.isSealed = true;
            if (_state.epilogueReport != null)
            {
                _state.epilogueReport.sealedDay = day;
            }

            OnCampaignSealed?.Invoke(_state.epilogueReport!);
            return true;
        }

        private EndingDef GetEndingOrDefault(string id)
        {
            if (_catalog.TryGetValue(id, out var def)) return def;
            return new EndingDef
            {
                id = id,
                title = "Unknown Closure",
                category = "survival",
                tone = "somber",
                summary = "The campaign concluded.",
                epilogue_text = "The story of the shelter came to a close amidst the silent ash."
            };
        }

        public EndgameSaveState CaptureState()
        {
            return new EndgameSaveState
            {
                schema_version = _state.schema_version,
                systemId = _state.systemId,
                phase = _state.phase,
                selectedEndingId = _state.selectedEndingId,
                isSealed = _state.isSealed,
                epilogueReport = _state.epilogueReport != null ? new CampaignEpilogueReport
                {
                    endingId = _state.epilogueReport.endingId,
                    endingTitle = _state.epilogueReport.endingTitle,
                    tone = _state.epilogueReport.tone,
                    mainEpilogueProse = _state.epilogueReport.mainEpilogueProse,
                    memorialTributes = new List<string>(_state.epilogueReport.memorialTributes),
                    factionReactions = new List<string>(_state.epilogueReport.factionReactions),
                    daysSurvived = _state.epilogueReport.daysSurvived,
                    livingSurvivors = _state.epilogueReport.livingSurvivors,
                    deceasedSurvivors = _state.epilogueReport.deceasedSurvivors,
                    finalMoraleAverage = _state.epilogueReport.finalMoraleAverage,
                    expeditionsCompleted = _state.epilogueReport.expeditionsCompleted,
                    sealedDay = _state.epilogueReport.sealedDay
                } : null
            };
        }

        public void RestoreState(EndgameSaveState saved)
        {
            if (saved == null) return;
            _state = new EndgameSaveState
            {
                schema_version = saved.schema_version,
                systemId = saved.systemId,
                phase = saved.phase,
                selectedEndingId = saved.selectedEndingId ?? string.Empty,
                isSealed = saved.isSealed,
                epilogueReport = saved.epilogueReport != null ? new CampaignEpilogueReport
                {
                    endingId = saved.epilogueReport.endingId ?? string.Empty,
                    endingTitle = saved.epilogueReport.endingTitle ?? string.Empty,
                    tone = saved.epilogueReport.tone ?? string.Empty,
                    mainEpilogueProse = saved.epilogueReport.mainEpilogueProse ?? string.Empty,
                    memorialTributes = new List<string>(saved.epilogueReport.memorialTributes ?? new List<string>()),
                    factionReactions = new List<string>(saved.epilogueReport.factionReactions ?? new List<string>()),
                    daysSurvived = saved.epilogueReport.daysSurvived,
                    livingSurvivors = saved.epilogueReport.livingSurvivors,
                    deceasedSurvivors = saved.epilogueReport.deceasedSurvivors,
                    finalMoraleAverage = saved.epilogueReport.finalMoraleAverage,
                    expeditionsCompleted = saved.epilogueReport.expeditionsCompleted,
                    sealedDay = saved.epilogueReport.sealedDay
                } : null
            };
        }
    }
}
