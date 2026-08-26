using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Endgame
{
    /// <summary>
    /// ASHFALL Epilogue Chronicle (item 16).
    ///
    /// Presentation-only DTOs that wrap an authoritative ending key from
    /// <see cref="Ashfall.Core.Muster.MusterSystem.ResolveEndingKey"/>
    /// into an ordered sequence of slides, survivor fate cards, metrics,
    /// and prose. The Core produces the chronicle deterministically from
    /// the same inputs every time so the same campaign state always
    /// yields the same ending slides and metrics.
    /// </summary>
    public sealed class EpilogueChronicleBuilder
    {
        public EpilogueChronicle Build(EpilogueChronicleInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var chronicle = new EpilogueChronicle
            {
                EndingKey = input.EndingKey ?? "unknown",
                GeneratedDay = input.Day,
                BuildSeed = input.BuildSeed,
                Title = TitleFor(input.EndingKey ?? "unknown"),
                Metrics = new List<EpilogueMetric>(input.Metrics ?? new List<EpilogueMetric>()),
                Slides = new List<EpilogueSlide>(input.Slides ?? new List<EpilogueSlide>()),
                FateCards = new List<SurvivorFateCard>(input.FateCards ?? new List<SurvivorFateCard>())
            };
            // Stable ordering: by slide index, then by survivor id.
            chronicle.Slides.Sort((a, b) => a.Order.CompareTo(b.Order));
            chronicle.FateCards.Sort((a, b) =>
                string.CompareOrdinal(a.SurvivorId, b.SurvivorId));
            chronicle.Metrics.Sort((a, b) =>
                string.CompareOrdinal(a.MetricId, b.MetricId));
            return chronicle;
        }

        private static string TitleFor(string endingKey)
        {
            if (string.IsNullOrEmpty(endingKey)) return "UNKNOWN ENDING";
            return endingKey.ToUpperInvariant() switch
            {
                "KNOWING" => "Knowing",
                "CULPABLE" => "Culpable",
                "REMEMBERING" => "Remembering",
                "FORGIVING" => "Forgiving",
                _ => endingKey
            };
        }
    }

    [Serializable]
    public sealed class EpilogueChronicleInput
    {
        public string EndingKey;
        public int Day;
        public int BuildSeed;
        public List<EpilogueSlide> Slides;
        public List<SurvivorFateCard> FateCards;
        public List<EpilogueMetric> Metrics;
    }

    [Serializable]
    public sealed class EpilogueChronicle
    {
        public string EndingKey;
        public string Title;
        public int GeneratedDay;
        public int BuildSeed;
        public List<EpilogueSlide> Slides = new List<EpilogueSlide>();
        public List<SurvivorFateCard> FateCards = new List<SurvivorFateCard>();
        public List<EpilogueMetric> Metrics = new List<EpilogueMetric>();
    }

    [Serializable]
    public sealed class EpilogueSlide
    {
        public int Order;
        public string Title;
        public string Prose;
        public string ArtAssetId;

        public EpilogueSlide() { }

        public EpilogueSlide(int order, string title, string prose, string? artAssetId = null)
        {
            Order = order;
            Title = title ?? string.Empty;
            Prose = prose ?? string.Empty;
            ArtAssetId = artAssetId ?? string.Empty;
        }
    }

    [Serializable]
    public sealed class SurvivorFateCard
    {
        public string SurvivorId;
        public string DisplayName;
        public string Fate;
        public bool Survived;

        public SurvivorFateCard() { }
    }

    [Serializable]
    public sealed class EpilogueMetric
    {
        public string MetricId;
        public float Value;
        public string DisplayLabel;

        public EpilogueMetric() { }

        public EpilogueMetric(string metricId, float value, string displayLabel)
        {
            MetricId = metricId ?? string.Empty;
            Value = value;
            DisplayLabel = displayLabel ?? string.Empty;
        }
    }
}
