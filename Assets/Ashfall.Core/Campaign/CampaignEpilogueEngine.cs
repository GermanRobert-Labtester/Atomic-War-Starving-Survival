// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Ashfall.Core.Campaign
{
    [Serializable]
    public sealed class CampaignEpilogueSnapshot
    {
        public int FinalDay { get; set; }
        public int SurvivorsAlive { get; set; }
        public int TotalCasualties { get; set; }
        public int StarvationDeaths { get; set; }
        public int DiseaseDeaths { get; set; }
        public int ArchivesDecrypted { get; set; }
        public int TechNodesCompleted { get; set; }
        public int CaptivesParoled { get; set; }
        public int CaptivesInterrogated { get; set; }
        public int PenalLaborShiftsRun { get; set; }
        public float AverageFreshnessConsumed { get; set; }
        public Dictionary<string, int> FactionStandings { get; set; } = new Dictionary<string, int>();
        public List<string> HistoricDecisions { get; set; } = new List<string>();
        public ulong CampaignSeed { get; set; } = 42;
    }

    [Serializable]
    public sealed class EpilogueChapter
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string NarrativeText { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class EpilogueChronicle
    {
        public string CampaignId { get; set; } = "ASHFALL_CAMPAIGN";
        public int TotalDays { get; set; }
        public List<EpilogueChapter> Chapters { get; set; } = new List<EpilogueChapter>();
        public CampaignEpilogueSnapshot FinalMetrics { get; set; } = new CampaignEpilogueSnapshot();

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        public string ToFormattedReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=================================================");
            sb.AppendLine($"         CHRONICLE OF THE VAULT — DAY {TotalDays}");
            sb.AppendLine("=================================================");
            sb.AppendLine();

            foreach (var chap in Chapters)
            {
                sb.AppendLine($"--- {chap.Title} ({chap.Category}) ---");
                sb.AppendLine(chap.NarrativeText);
                sb.AppendLine();
                if (chap.Highlights.Count > 0)
                {
                    sb.AppendLine("Key Historical Markers:");
                    foreach (var h in chap.Highlights)
                    {
                        sb.AppendLine($"  • {h}");
                    }
                    sb.AppendLine();
                }
            }

            sb.AppendLine("=================================================");
            sb.AppendLine($"Survivors Living: {FinalMetrics.SurvivorsAlive} | Starvation: {FinalMetrics.StarvationDeaths} | Disease: {FinalMetrics.DiseaseDeaths}");
            sb.AppendLine($"Archives Decrypted: {FinalMetrics.ArchivesDecrypted} | Captives Paroled: {FinalMetrics.CaptivesParoled}");
            sb.AppendLine("=================================================");
            return sb.ToString();
        }
    }

    public sealed class CampaignEpilogueEngine
    {
        private readonly CampaignEpilogueCatalog _catalog;

        public CampaignEpilogueEngine(CampaignEpilogueCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public EpilogueChronicle GenerateChronicle(CampaignEpilogueSnapshot snapshot)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

            var chronicle = new EpilogueChronicle
            {
                TotalDays = snapshot.FinalDay,
                FinalMetrics = snapshot
            };

            var rng = new SeededRng((int)(snapshot.CampaignSeed & 0x7FFFFFFF));

            // 1. Demographics & Survival
            var demoVignette = SelectVignette("demographics", v =>
                snapshot.SurvivorsAlive >= v.min_survivors &&
                snapshot.SurvivorsAlive <= v.max_survivors &&
                snapshot.StarvationDeaths <= v.max_starvation_deaths, rng);

            if (demoVignette != null)
            {
                chronicle.Chapters.Add(new EpilogueChapter
                {
                    Category = "Demographics",
                    Title = demoVignette.title,
                    NarrativeText = demoVignette.narrative,
                    Highlights = new List<string>
                    {
                        $"Final population: {snapshot.SurvivorsAlive} survivors",
                        $"Total casualties across campaign: {snapshot.TotalCasualties}",
                        $"Starvation fatalities: {snapshot.StarvationDeaths}"
                    }
                });
            }

            // 2. Governance & Justice
            var govVignette = SelectVignette("governance", v =>
                snapshot.CaptivesParoled >= v.min_paroled_captives &&
                snapshot.PenalLaborShiftsRun <= v.max_penal_shifts, rng);

            if (govVignette != null)
            {
                chronicle.Chapters.Add(new EpilogueChapter
                {
                    Category = "Governance",
                    Title = govVignette.title,
                    NarrativeText = govVignette.narrative,
                    Highlights = new List<string>
                    {
                        $"Hostiles paroled and integrated: {snapshot.CaptivesParoled}",
                        $"Penal labor shifts enforced: {snapshot.PenalLaborShiftsRun}",
                        $"Total interrogations logged: {snapshot.CaptivesInterrogated}"
                    }
                });
            }

            // 3. Technology & Archives
            var techVignette = SelectVignette("technology", v =>
                snapshot.ArchivesDecrypted >= v.min_archives_decrypted, rng);

            if (techVignette != null)
            {
                chronicle.Chapters.Add(new EpilogueChapter
                {
                    Category = "Technology",
                    Title = techVignette.title,
                    NarrativeText = techVignette.narrative,
                    Highlights = new List<string>
                    {
                        $"Pre-war archives deciphered: {snapshot.ArchivesDecrypted}",
                        $"Knowledge research milestones: {snapshot.TechNodesCompleted}"
                    }
                });
            }

            // 4. Sustenance & Preservation
            var foodVignette = SelectVignette("sustenance", v =>
                snapshot.StarvationDeaths >= v.min_starvation_deaths &&
                snapshot.StarvationDeaths <= v.max_starvation_deaths, rng);

            if (foodVignette != null)
            {
                chronicle.Chapters.Add(new EpilogueChapter
                {
                    Category = "Sustenance",
                    Title = foodVignette.title,
                    NarrativeText = foodVignette.narrative,
                    Highlights = new List<string>
                    {
                        $"Starvation fatalities: {snapshot.StarvationDeaths}",
                        $"Disease fatalities: {snapshot.DiseaseDeaths}"
                    }
                });
            }

            return chronicle;
        }

        private EpilogueVignetteDef? SelectVignette(
            string category,
            Func<EpilogueVignetteDef, bool> predicate,
            ISeededRng rng)
        {
            var matches = new List<EpilogueVignetteDef>();
            foreach (var v in _catalog.GetAllVignettes())
            {
                if (string.Equals(v.category, category, StringComparison.OrdinalIgnoreCase) && predicate(v))
                {
                    matches.Add(v);
                }
            }

            if (matches.Count == 0) return null;

            // Sort by priority descending
            matches.Sort((a, b) => b.priority.CompareTo(a.priority));

            int highestPriority = matches[0].priority;
            var topCandidates = matches.FindAll(m => m.priority == highestPriority);

            if (topCandidates.Count == 1)
                return topCandidates[0];

            int pick = rng.Next(0, topCandidates.Count);
            return topCandidates[pick];
        }
    }
}
