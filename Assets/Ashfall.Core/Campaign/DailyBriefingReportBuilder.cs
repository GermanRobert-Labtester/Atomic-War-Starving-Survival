using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Daily Briefing — pure Core report builder (item 01).
    ///
    /// Given a typed <see cref="DailyBriefingInputs"/> snapshot of every daily
    /// subsystem, produces a deterministic <see cref="DailyBriefingReport"/>
    /// whose entries are sorted by category, then survivor/item id, then
    /// event order. Core must not reference host systems: every input is a
    /// plain DTO so the report can be unit-tested without Godot/Unity.
    /// </summary>
    public static class DailyBriefingReportBuilder
    {
        public static DailyBriefingReport Build(DailyBriefingInputs inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            var r = new DailyBriefingReport
            {
                Day = inputs.Day,
                BuildSeed = inputs.BuildSeed,
                Title = "DAY " + inputs.Day + " BRIEFING",
                GeneratedUtc = inputs.GeneratedUtc
            };

            foreach (var s in SurvivorList(inputs)) r.Sections.Add(s);
            foreach (var s in ResourceList(inputs)) r.Sections.Add(s);
            foreach (var s in WeatherList(inputs)) r.Sections.Add(s);
            foreach (var s in RadioList(inputs)) r.Sections.Add(s);
            foreach (var s in ExpeditionList(inputs)) r.Sections.Add(s);
            foreach (var s in DeathList(inputs)) r.Sections.Add(s);
            foreach (var s in WarningList(inputs)) r.Sections.Add(s);

            return r;
        }

        private static IEnumerable<DailyBriefingSection> SurvivorList(DailyBriefingInputs i)
        {
            var entries = new List<DailyBriefingEntry>();
            if (i.SurvivorChanges != null)
                entries.AddRange(i.SurvivorChanges);
            entries.Sort(StableEntrySort);
            if (entries.Count == 0) yield break;
            yield return new DailyBriefingSection("Survivor Changes", entries);
        }

        private static IEnumerable<DailyBriefingSection> ResourceList(DailyBriefingInputs i)
        {
            var entries = new List<DailyBriefingEntry>();
            if (i.ResourceConsumption != null)
                entries.AddRange(i.ResourceConsumption);
            entries.Sort(StableEntrySort);
            if (entries.Count == 0) yield break;
            yield return new DailyBriefingSection("Resource Consumption", entries);
        }

        private static IEnumerable<DailyBriefingSection> WeatherList(DailyBriefingInputs i)
        {
            if (i.WeatherForecast == null || i.WeatherForecast.Count == 0) yield break;
            var entries = new List<DailyBriefingEntry>(i.WeatherForecast);
            entries.Sort(StableEntrySort);
            yield return new DailyBriefingSection("Weather Forecast", entries);
        }

        private static IEnumerable<DailyBriefingSection> RadioList(DailyBriefingInputs i)
        {
            if (i.RadioIntercepts == null || i.RadioIntercepts.Count == 0) yield break;
            var entries = new List<DailyBriefingEntry>(i.RadioIntercepts);
            entries.Sort(StableEntrySort);
            yield return new DailyBriefingSection("Radio Intercepts", entries);
        }

        private static IEnumerable<DailyBriefingSection> ExpeditionList(DailyBriefingInputs i)
        {
            if (i.ExpeditionMilestones == null || i.ExpeditionMilestones.Count == 0) yield break;
            var entries = new List<DailyBriefingEntry>(i.ExpeditionMilestones);
            entries.Sort(StableEntrySort);
            yield return new DailyBriefingSection("Expedition Milestones", entries);
        }

        private static IEnumerable<DailyBriefingSection> DeathList(DailyBriefingInputs i)
        {
            if (i.Deaths == null || i.Deaths.Count == 0) yield break;
            var entries = new List<DailyBriefingEntry>(i.Deaths);
            entries.Sort(StableEntrySort);
            yield return new DailyBriefingSection("Deaths", entries);
        }

        private static IEnumerable<DailyBriefingSection> WarningList(DailyBriefingInputs i)
        {
            if (i.Warnings == null || i.Warnings.Count == 0) yield break;
            var entries = new List<DailyBriefingEntry>(i.Warnings);
            entries.Sort(StableEntrySort);
            yield return new DailyBriefingSection("Warnings", entries);
        }

        private static int StableEntrySort(DailyBriefingEntry a, DailyBriefingEntry b)
        {
            int c = string.CompareOrdinal(a.Category, b.Category);
            if (c != 0) return c;
            c = string.CompareOrdinal(a.PrimaryId, b.PrimaryId);
            if (c != 0) return c;
            c = string.CompareOrdinal(a.SecondaryId, b.SecondaryId);
            if (c != 0) return c;
            return a.Order.CompareTo(b.Order);
        }
    }

    /// <summary>Pure DTO inputs to the briefing builder.</summary>
    [Serializable]
    public sealed class DailyBriefingInputs
    {
        public int Day;
        public int BuildSeed;
        public string GeneratedUtc;
        public List<DailyBriefingEntry> SurvivorChanges;
        public List<DailyBriefingEntry> ResourceConsumption;
        public List<DailyBriefingEntry> WeatherForecast;
        public List<DailyBriefingEntry> RadioIntercepts;
        public List<DailyBriefingEntry> ExpeditionMilestones;
        public List<DailyBriefingEntry> Deaths;
        public List<DailyBriefingEntry> Warnings;
    }

    /// <summary>One row in a briefing section.</summary>
    [Serializable]
    public sealed class DailyBriefingEntry
    {
        public string Category;
        public string PrimaryId;
        public string SecondaryId;
        public string Text;
        public int Order;
        public float Numeric;

        public DailyBriefingEntry() { }

        public DailyBriefingEntry(string category, string primaryId,
            string text, int order =0, string? secondaryId = null, float numeric = 0f)
        {
            Category = category ?? string.Empty;
            PrimaryId = primaryId ?? string.Empty;
            SecondaryId = secondaryId ?? string.Empty;
            Text = text ?? string.Empty;
            Order = order;
            Numeric = numeric;
        }
    }

    /// <summary>One section of the briefing.</summary>
    [Serializable]
    public sealed class DailyBriefingSection
    {
        public string Title;
        public DailyBriefingEntry[] Entries;

        public DailyBriefingSection() { }

        public DailyBriefingSection(string title, IReadOnlyList<DailyBriefingEntry> entries)
        {
            Title = title ?? string.Empty;
            Entries = new DailyBriefingEntry[entries?.Count ?? 0];
            if (entries != null)
                for (int i = 0; i < entries.Count; i++)
                    Entries[i] = entries[i];
        }
    }

    /// <summary>Final report shown in the briefing modal.</summary>
    [Serializable]
    public sealed class DailyBriefingReport
    {
        public int Day;
        public int BuildSeed;
        public string Title;
        public string GeneratedUtc;
        public List<DailyBriefingSection> Sections = new List<DailyBriefingSection>();

        public bool IsEmpty
        {
            get
            {
                if (Sections == null) return true;
                for (int i = 0; i < Sections.Count; i++)
                    if (Sections[i]?.Entries?.Length > 0) return false;
                return true;
            }
        }

        public int TotalEntries
        {
            get
            {
                int n = 0;
                if (Sections == null) return 0;
                for (int i = 0; i < Sections.Count; i++)
                    n += Sections[i]?.Entries?.Length ?? 0;
                return n;
            }
        }
    }
}
