using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS8618

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Daily Briefing — pure Core report builder.
    ///
    /// Produces a deterministic <see cref="DailyBriefingReport"/> from typed day events
    /// (<see cref="DayStateChangeEvent"/>) or fallback <see cref="DailyBriefingInputs"/>.
    /// Entries are deduplicated, ordered by severity and category, and truncated with overflow
    /// indicators when event count is high.
    /// </summary>
    public static class DailyBriefingReportBuilder
    {
        public const int DefaultMaxEntriesPerSection = 10;

        /// <summary>
        /// Builds a deterministic daily briefing report directly from typed day events.
        /// </summary>
        public static DailyBriefingReport BuildFromDayEvents(
            int day,
            int buildSeed,
            IEnumerable<DayStateChangeEvent>? events,
            int maxEntriesPerSection = DefaultMaxEntriesPerSection)
        {
            var r = new DailyBriefingReport
            {
                Day = day,
                BuildSeed = buildSeed,
                Title = $"DAY {day} BRIEFING",
                GeneratedUtc = string.Empty
            };

            if (events == null) return r;

            var deaths = new List<DailyBriefingEntry>();
            var warnings = new List<DailyBriefingEntry>();
            var subterranean = new List<DailyBriefingEntry>();
            var survivorChanges = new List<DailyBriefingEntry>();
            var shelterSocial = new List<DailyBriefingEntry>();
            var resourceConsumption = new List<DailyBriefingEntry>();
            var production = new List<DailyBriefingEntry>();
            var weatherForecast = new List<DailyBriefingEntry>();
            var radioIntercepts = new List<DailyBriefingEntry>();
            var expeditionMilestones = new List<DailyBriefingEntry>();

            int order = 0;
            foreach (var evt in events)
            {
                if (evt == null) continue;
                order++;

                switch (evt.Kind)
                {
                    case "survivor_perished":
                        deaths.Add(new DailyBriefingEntry("Deaths", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"{evt.PrimaryId} has perished." : $"{evt.PrimaryId} perished: {evt.SecondaryId}", order: order));
                        break;

                    case "shelter_consequence":
                    case "hazard_warning":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"{evt.PrimaryId} warning." : $"{evt.PrimaryId}: {evt.SecondaryId}", order: order, numeric: evt.Numeric));
                        break;

                    case "survivor_condition":
                        if (string.Equals(evt.SecondaryId, "critical", StringComparison.OrdinalIgnoreCase) || evt.Numeric >= 80f)
                            warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId, $"{evt.PrimaryId} is in critical condition ({evt.SecondaryId}, {evt.Numeric:F0}%).", order: order, numeric: evt.Numeric));
                        else
                            survivorChanges.Add(new DailyBriefingEntry("Survivor Changes", evt.PrimaryId, $"{evt.PrimaryId} condition: {evt.SecondaryId}.", order: order, numeric: evt.Numeric));
                        break;

                    case "consumed_rations":
                        resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", evt.PrimaryId,
                            $"{evt.PrimaryId}: {evt.Numeric:F0} consumed.", order: order, numeric: evt.Numeric));
                        break;

                    case "resource_delta":
                        resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", evt.PrimaryId,
                            $"{evt.PrimaryId}: {(evt.Numeric >= 0 ? "+" : "")}{evt.Numeric:F0}", order: order, numeric: evt.Numeric));
                        break;

                    case "weather_condition":
                    case "weather_ticked":
                        weatherForecast.Add(new DailyBriefingEntry("Weather Forecast", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Surface weather: {evt.PrimaryId}" : $"Surface weather: {evt.PrimaryId} ({evt.SecondaryId})", order: order));
                        break;

                    case "radio_intercept":
                    case "radio_transmission":
                        radioIntercepts.Add(new DailyBriefingEntry("Radio Intercepts", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Intercept on {evt.PrimaryId}" : $"[{evt.PrimaryId}] {evt.SecondaryId}", order: order));
                        break;

                    case "radio_intercept_decrypted":
                        radioIntercepts.Add(new DailyBriefingEntry("Radio Intercepts", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Decrypted signal: {evt.PrimaryId}" : $"[{evt.PrimaryId}] Decrypted: {evt.SecondaryId}", order: order));
                        break;

                    case "radio_distress_active":
                        radioIntercepts.Add(new DailyBriefingEntry("Radio Intercepts", evt.PrimaryId,
                            $"Active SOS: {evt.PrimaryId} ({evt.Numeric:F0} days remaining).", order: order, numeric: evt.Numeric));
                        break;

                    case "radio_distress_expiring":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"CRITICAL: Distress signal {evt.PrimaryId} expires in {evt.Numeric:F0} day(s)!", order: order, numeric: evt.Numeric));
                        break;

                    case "radio_location_triangulated":
                        radioIntercepts.Add(new DailyBriefingEntry("Radio Intercepts", evt.PrimaryId,
                            $"Triangulated coordinates for {evt.SecondaryId} from {evt.PrimaryId}.", order: order));
                        break;

                    case "social_privacy_warning":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"Severe privacy fatigue in {evt.PrimaryId} ({evt.Numeric:F0}‰) - high friction risk!", order: order, numeric: evt.Numeric));
                        break;

                    case "social_dispute_unresolved":
                        shelterSocial.Add(new DailyBriefingEntry("Shelter Social", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Unresolved dispute: {evt.PrimaryId}" : $"Unresolved dispute in {evt.SecondaryId}: {evt.PrimaryId}", order: order));
                        break;

                    case "social_dispute_mediated":
                        shelterSocial.Add(new DailyBriefingEntry("Shelter Social", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Dispute mediated: {evt.PrimaryId}" : $"Dispute in {evt.SecondaryId} mediated by {evt.PrimaryId}.", order: order));
                        break;

                    case "subterranean_methane_warning":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"Methane accumulation in {evt.PrimaryId} at {evt.Numeric:F0} PPM!", order: order, numeric: evt.Numeric));
                        break;

                    case "subterranean_flood_warning":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"Flood water depth in {evt.PrimaryId} at {evt.Numeric:F0}‰!", order: order, numeric: evt.Numeric));
                        break;

                    case "subterranean_shoring_warning":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"Shoring integrity degraded in {evt.PrimaryId} ({evt.Numeric:F0}‰)!", order: order, numeric: evt.Numeric));
                        break;

                    case "subterranean_cave_in":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"CRITICAL: Cave-in collapse in {evt.PrimaryId}! Miners trapped!", order: order, numeric: evt.Numeric));
                        break;

                    case "subterranean_rescue_active":
                        subterranean.Add(new DailyBriefingEntry("Subterranean Operations", evt.PrimaryId,
                            $"Rescue underway in {evt.PrimaryId}: {evt.Numeric:F0} labor ticks remaining.", order: order, numeric: evt.Numeric));
                        break;

                    case "subterranean_rescue_completed":
                        subterranean.Add(new DailyBriefingEntry("Subterranean Operations", evt.PrimaryId,
                            $"Rescue in {evt.PrimaryId} completed successfully! Miners extracted.", order: order));
                        break;

                    case "subterranean_rescue_failed":
                        deaths.Add(new DailyBriefingEntry("Deaths", evt.PrimaryId,
                            $"Rescue in {evt.PrimaryId} failed. Trapped miners perished.", order: order));
                        break;

                    case "expedition_milestone":
                    case "expeditions_caravans_ticked":
                        if (!string.IsNullOrEmpty(evt.PrimaryId) && !string.Equals(evt.PrimaryId, "none", StringComparison.OrdinalIgnoreCase))
                        {
                            expeditionMilestones.Add(new DailyBriefingEntry("Expedition Milestones", evt.PrimaryId,
                                string.IsNullOrEmpty(evt.SecondaryId) ? $"Expedition update: {evt.PrimaryId}" : $"{evt.PrimaryId}: {evt.SecondaryId}", order: order));
                        }
                        break;

                    case "crafting_completed":
                    case "crafting_production":
                        production.Add(new DailyBriefingEntry("Production & Maintenance", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.PrimaryId) ? "Crafting work advanced." : $"Crafting completed: {evt.PrimaryId}", order: order, numeric: evt.Numeric));
                        break;

                    case "workshop_job_completed":
                        production.Add(new DailyBriefingEntry("Production & Maintenance", evt.PrimaryId,
                            string.IsNullOrEmpty(evt.SecondaryId) ? $"Workshop completed: {evt.PrimaryId} ({evt.Numeric:F0} units)." : $"Workshop completed {evt.SecondaryId}: {evt.PrimaryId} ({evt.Numeric:F0} units).", order: order, numeric: evt.Numeric));
                        break;

                    case "workshop_machine_degraded":
                        warnings.Add(new DailyBriefingEntry("Warnings", evt.PrimaryId,
                            $"Machine tooling degraded in {evt.PrimaryId} ({evt.Numeric * 100f:F0}% health).", order: order, numeric: evt.Numeric));
                        break;

                    case "workshop_machine_overhauled":
                        production.Add(new DailyBriefingEntry("Production & Maintenance", evt.PrimaryId,
                            $"Machine overhaul completed in {evt.PrimaryId}.", order: order, numeric: evt.Numeric));
                        break;
                }
            }

            AddSectionIfNotEmpty(r, "Deaths", deaths, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Warnings", warnings, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Subterranean Operations", subterranean, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Survivor Changes", survivorChanges, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Shelter Social", shelterSocial, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Resource Consumption", resourceConsumption, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Production & Maintenance", production, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Weather Forecast", weatherForecast, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Radio Intercepts", radioIntercepts, maxEntriesPerSection);
            AddSectionIfNotEmpty(r, "Expedition Milestones", expeditionMilestones, maxEntriesPerSection);

            return r;
        }

        public static DailyBriefingReport Build(DailyBriefingInputs inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            var r = new DailyBriefingReport
            {
                Day = inputs.Day,
                BuildSeed = inputs.BuildSeed,
                Title = $"DAY {inputs.Day} BRIEFING",
                GeneratedUtc = string.Empty
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

        private static void AddSectionIfNotEmpty(
            DailyBriefingReport report,
            string title,
            List<DailyBriefingEntry> entries,
            int maxEntries)
        {
            if (entries.Count == 0) return;

            // Deduplicate entries by (Category, PrimaryId, Text)
            var deduplicated = new List<DailyBriefingEntry>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var e in entries)
            {
                string key = $"{e.Category}|{e.PrimaryId}|{e.Text}";
                if (seen.Add(key))
                    deduplicated.Add(e);
            }

            deduplicated.Sort(StableEntrySort);

            // Apply overflow rule
            if (deduplicated.Count > maxEntries)
            {
                int overflow = deduplicated.Count - maxEntries;
                var trimmed = deduplicated.Take(maxEntries).ToList();
                trimmed.Add(new DailyBriefingEntry(title, "overflow", $"...and {overflow} more items.", order: 999));
                report.Sections.Add(new DailyBriefingSection(title, trimmed));
            }
            else
            {
                report.Sections.Add(new DailyBriefingSection(title, deduplicated));
            }
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
            string text, int order = 0, string? secondaryId = null, float numeric = 0f)
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
