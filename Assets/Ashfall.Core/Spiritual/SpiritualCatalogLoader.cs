using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Spiritual
{
    public sealed class SpiritualCatalog
    {
        public List<SpiritualRitualDefinition> Rituals { get; } = new List<SpiritualRitualDefinition>();
        public List<MemorialRiteDefinition> MemorialRites { get; } = new List<MemorialRiteDefinition>();
        public List<BeliefMovementDefinition> Movements { get; } = new List<BeliefMovementDefinition>();

        private readonly Dictionary<string, SpiritualRitualDefinition> _ritualsById =
            new Dictionary<string, SpiritualRitualDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, MemorialRiteDefinition> _memorialRitesById =
            new Dictionary<string, MemorialRiteDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, BeliefMovementDefinition> _movementsById =
            new Dictionary<string, BeliefMovementDefinition>(StringComparer.Ordinal);

        public void Index()
        {
            _ritualsById.Clear();
            foreach (var r in Rituals)
                if (r != null && !string.IsNullOrEmpty(r.Id)) _ritualsById[r.Id] = r;

            _memorialRitesById.Clear();
            foreach (var m in MemorialRites)
                if (m != null && !string.IsNullOrEmpty(m.Id)) _memorialRitesById[m.Id] = m;

            _movementsById.Clear();
            foreach (var b in Movements)
                if (b != null && !string.IsNullOrEmpty(b.Id)) _movementsById[b.Id] = b;
        }

        public SpiritualRitualDefinition? GetRitual(string id) =>
            _ritualsById.TryGetValue(id, out var r) ? r : null;

        public MemorialRiteDefinition? GetMemorialRite(string id) =>
            _memorialRitesById.TryGetValue(id, out var m) ? m : null;

        public BeliefMovementDefinition? GetMovement(string id) =>
            _movementsById.TryGetValue(id, out var b) ? b : null;
    }

    public static class SpiritualCatalogLoader
    {
        private class RitualWrapper
        {
            public int schema_version { get; set; }
            public List<SpiritualRitualDefinition>? rituals { get; set; }
        }

        private class MemorialRiteWrapper
        {
            public int schema_version { get; set; }
            public List<MemorialRiteDefinition>? memorial_rites { get; set; }
        }

        private class BeliefMovementWrapper
        {
            public int schema_version { get; set; }
            public List<BeliefMovementDefinition>? movements { get; set; }
        }

        public static SpiritualCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(dataDir)) throw new ArgumentNullException(nameof(dataDir));
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            var catalog = new SpiritualCatalog();

            // 1. spiritual_rituals.json
            string ritualsPath = fileIO.Combine(dataDir, "spiritual_rituals.json");
            if (fileIO.FileExists(ritualsPath))
            {
                string json = fileIO.ReadAllText(ritualsPath);
                var wrapper = serializer.Deserialize<RitualWrapper>(json);
                if (wrapper?.rituals != null)
                {
                    catalog.Rituals.AddRange(wrapper.rituals);
                }
            }

            // 2. memorial_rites.json
            string memorialPath = fileIO.Combine(dataDir, "memorial_rites.json");
            if (fileIO.FileExists(memorialPath))
            {
                string json = fileIO.ReadAllText(memorialPath);
                var wrapper = serializer.Deserialize<MemorialRiteWrapper>(json);
                if (wrapper?.memorial_rites != null)
                {
                    catalog.MemorialRites.AddRange(wrapper.memorial_rites);
                }
            }

            // 3. belief_movements.json
            string movementsPath = fileIO.Combine(dataDir, "belief_movements.json");
            if (fileIO.FileExists(movementsPath))
            {
                string json = fileIO.ReadAllText(movementsPath);
                var wrapper = serializer.Deserialize<BeliefMovementWrapper>(json);
                if (wrapper?.movements != null)
                {
                    catalog.Movements.AddRange(wrapper.movements);
                }
            }

            catalog.Index();
            return catalog;
        }
    }
}
