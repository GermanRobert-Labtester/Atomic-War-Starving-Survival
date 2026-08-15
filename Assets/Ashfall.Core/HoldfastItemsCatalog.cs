using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>Holdfast item (trade surface). Matches the terminal/catalog contract.</summary>
    public sealed class HoldfastItemDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public float TradeValue { get; }
        public float Weight { get; }
        public string Type { get; }
        public int StackMax { get; }

        public string id => Id;
        public string displayName => DisplayName;
        public string description => Description;
        public float tradeValue => TradeValue;
        public float weight => Weight;

        public HoldfastItemDefinition(string id, string displayName, string description, float tradeValue, float weight = 1f, string type = "resource", int stackMax = 99)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Description = description ?? string.Empty;
            TradeValue = tradeValue;
            Weight = weight;
            Type = type ?? "resource";
            StackMax = stackMax > 0 ? stackMax : 99;
        }
    }

    /// <summary>Immutable-after-load Holdfast item catalog. Enumeration ordinal-sorted;
    /// ID lookup dictionary-backed (independent of JSON array order).</summary>
    public sealed class HoldfastItemsCatalog
    {
        private readonly Dictionary<string, HoldfastItemDefinition> _byId =
            new Dictionary<string, HoldfastItemDefinition>(StringComparer.Ordinal);

        public int Count => _byId.Count;
        public bool IsValid => _byId.Count > 0;

        public static HoldfastItemsCatalog Empty() => new HoldfastItemsCatalog();

        public void Register(HoldfastItemDefinition def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Id) && !_byId.ContainsKey(def.Id))
                _byId[def.Id] = def;
        }

        public HoldfastItemDefinition GetById(string id)
            => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var d) ? d : null);

        public bool Contains(string id) => GetById(id) != null;

        public IReadOnlyCollection<string> Ids => _byId.Keys;

        public IReadOnlyList<HoldfastItemDefinition> Items => All();

        public List<HoldfastItemDefinition> All()
        {
            var list = new List<HoldfastItemDefinition>(_byId.Values);
            list.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
            return list;
        }
    }
}
