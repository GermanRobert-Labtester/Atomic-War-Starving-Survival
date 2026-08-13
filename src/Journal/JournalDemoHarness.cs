using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>Concrete author for the Godot stub (no full survivor sim yet).</summary>
    public class DemoSurvivor : ISurvivorAuthor
    {
        public string Id { get; }
        public string DisplayName { get; }
        public RiskBiasTrait RiskBias { get; }

        public DemoSurvivor(string id, string displayName, RiskBiasTrait bias)
        {
            Id = id;
            DisplayName = displayName;
            RiskBias = bias;
        }
    }

    /// <summary>
    /// First-run seeding for the journal: writes the opening-day discoveries in
    /// the survivors' own voices and unlocks the first codex rows. Every id used
    /// here comes from the StreamingAssets catalogs — nothing is invented.
    /// Only runs when no journal save exists.
    /// </summary>
    public static class JournalDemoHarness
    {
        private static readonly RiskBiasTrait[] s_biases =
        {
            RiskBiasTrait.Realist,
            RiskBiasTrait.Cautious,
            RiskBiasTrait.Paranoid,
            RiskBiasTrait.Fatalist
        };

        /// <summary>Seed a fresh journal. Returns the day the simulation should be at.</summary>
        public static int Seed(JournalSystem journal, JournalCatalogs catalogs)
        {
            var authors = BuildAuthors(catalogs);
            var crew = authors.Count > 0 ? authors.Values.ToList() : new List<DemoSurvivor>();

            int day = 1;
            journal.TryDiscover(KnowledgeKeys.HighCo2, At(crew, 0), day++);
            journal.TryDiscover(KnowledgeKeys.HasSeenRadiation, At(crew, 1), day++);
            journal.TryDiscover(KnowledgeKeys.HasExperiencedStorm, At(crew, 2), day++);
            journal.TryDiscover(KnowledgeKeys.FilterFailing, At(crew, 3), day++);

            if (catalogs != null)
            {
                for (int i = 0; i < catalogs.Survivors.Count && i < 3; i++)
                {
                    string? id = catalogs.Survivors[i].id;
                    if (!string.IsNullOrEmpty(id)) journal.UnlockSurvivorMet(id);
                }
                for (int i = 0; i < catalogs.Items.Count && i < 3; i++)
                {
                    string? id = catalogs.Items[i].id;
                    if (!string.IsNullOrEmpty(id)) journal.UnlockItemSeen(id);
                }
                if (catalogs.Locations.Count > 0)
                {
                    string? id = catalogs.Locations[0].id;
                    if (!string.IsNullOrEmpty(id)) journal.UnlockLocationVisited(id);
                }
                if (catalogs.Events.Count > 0)
                {
                    string? id = catalogs.Events[0].id;
                    if (!string.IsNullOrEmpty(id)) journal.UnlockEventFired(id);
                }
            }
            return day - 1; // last day actually written
        }

        private static DemoSurvivor? At(List<DemoSurvivor> crew, int index)
        {
            if (crew.Count == 0) return null;
            return crew[index % crew.Count];
        }

        private static Dictionary<string, DemoSurvivor> BuildAuthors(JournalCatalogs catalogs)
        {
            var authors = new Dictionary<string, DemoSurvivor>();
            if (catalogs?.Survivors == null) return authors;
            int bias = 0;
            for (int i = 0; i < catalogs.Survivors.Count; i++)
            {
                var s = catalogs.Survivors[i];
                if (string.IsNullOrEmpty(s.id)) continue;
                authors[s.id] = new DemoSurvivor(
                    s.id,
                    s.displayName ?? s.id,
                    s_biases[bias % s_biases.Length]);
                bias++;
            }
            return authors;
        }
    }
}
