using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;

using Ashfall.Core.IO;
namespace Ashfall.Core.Foundry
{
    // ---------------------------------------------------------------------
    // Static authored production catalog — foundry_production.json
    // Typed ownership for game rules. Do NOT hide rules in notes/tags.
    // ---------------------------------------------------------------------

    /// <summary>One ingredient line of a foundry production recipe.</summary>
    [Serializable]
    public sealed class FoundryIngredientEntry
    {
        public string item_id = string.Empty;
        public int amount = 1;
    }

    /// <summary>
    /// One castable product. All costs and consequences are typed fields so the
    /// runtime can enforce them deterministically. `treaty_id`/`quota_amount`
    /// bind a product to an authored treaty obligation (rail spikes, wheels,
    /// acid pipes) without duplicating the treaty itself.
    /// </summary>
    [Serializable]
    public sealed class FoundryProductEntry
    {
        public string product_id = string.Empty;
        public string display_name = string.Empty;

        /// <summary>
        /// Sink category the product feeds: agricultural_tool | structural_beam |
        /// ice_anchor | winch_drum | brine_resistant_pipe | repair_plate |
        /// bracket_fastener | water_component | heavy_tool | heavy_alloy_part |
        /// defense_plate.
        /// </summary>
        public string category = string.Empty;

        public string result_item_id = string.Empty;
        public int result_amount = 1;
        public List<FoundryIngredientEntry> ingredients = new List<FoundryIngredientEntry>();

        /// <summary>Total human labour-hours across the heat (spread over workers).</summary>
        public float labor_hours = 0f;

        /// <summary>Furnace time in hours from tap to pour completion.</summary>
        public float cast_hours = 0f;

        /// <summary>Coal/charcoal units consumed by the charge.</summary>
        public int fuel_units = 0;

        /// <summary>Water litres demanded by the heat (cooling + sand prep).</summary>
        public int water_litres = 0;

        /// <summary>Typical skill (0..1) the recipe assumes; deviation shifts quality.</summary>
        public float skill_target = 0.5f;

        /// <summary>Baseline quality score (0..100) the recipe is balanced around.</summary>
        public float quality_target = 70f;

        /// <summary>Optional authored treaty obligation (exact treaty id).</summary>
        public string treaty_id = string.Empty;

        /// <summary>Units of this product owed per treaty assessment cycle (0 = none).</summary>
        public int quota_amount = 0;

        /// <summary>Human-readable sink label for UI (authored context only).</summary>
        public string sink = string.Empty;

        public string notes = string.Empty;
        public string[] tags = Array.Empty<string>();
    }

    [Serializable]
    public sealed class FoundryProductionFile
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<FoundryProductEntry> products = new List<FoundryProductEntry>();
    }

    // ---------------------------------------------------------------------
    // Static faction registry entry — foundry_faction.json
    // Registers the District 8 works faction id (faction_silent_foundry).
    // ---------------------------------------------------------------------

    /// <summary>A named relationship to another faction; typed, not lore prose.</summary>
    [Serializable]
    public sealed class FoundryFactionRelation
    {
        public string faction_id = string.Empty;
        public string stance = string.Empty;   // ally | trade_partner | rival | internal
        public string notes = string.Empty;
    }

    [Serializable]
    public sealed class FoundryFactionEntry
    {
        public string faction_id = string.Empty;
        public string display_name = string.Empty;
        public string short_name = string.Empty;
        public string identity = string.Empty;
        public string icon_path = string.Empty;
        public string[] internal_divisions = Array.Empty<string>();
        public List<FoundryFactionRelation> relationships = new List<FoundryFactionRelation>();
        public string[] tags = Array.Empty<string>();
    }

    // ---------------------------------------------------------------------
    // Loader
    // ---------------------------------------------------------------------

    /// <summary>
    /// Engine-agnostic loader for the two Foundry static catalogs. Reads the
    /// exact snake_case JSON schema authored in StreamingAssets/Data.
    /// </summary>
    public static class SilentFoundryCatalogLoader
    {
        public const string ProductionFileName = "foundry_production.json";
        public const string FactionFileName = "foundry_faction.json";
        public const string AccordsFileName = "foundry_accords.json";

        /// <summary>
        /// District 8 accord ratification days (treaty id → ratified day) from
        /// foundry_accords.json. Same schema as the narrative treaty corpus, but
        /// authored for the live Sector 4 / District 8 campaign.
        /// </summary>
        public static Dictionary<string, int> LoadAccordRatificationDays(
            string dataDirectory,
            IFileIO files = null!,
            IJsonSerializer serializer = null!)
        {
            var ratification = new Dictionary<string, int>(StringComparer.Ordinal);
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            string path = Path.Combine(dataDirectory, AccordsFileName);
            if (!files.FileExists(path)) return ratification;
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return ratification;
            try
            {
                var file = serializer.Deserialize<RegionalTreatiesFile>(text);
                if (file?.treaties == null) return ratification;
                for (int i = 0; i < file.treaties.Count; i++)
                {
                    var t = file.treaties[i];
                    if (t != null && !string.IsNullOrEmpty(t.treaty_id) && t.ratified_day > 0)
                        ratification[t.treaty_id] = t.ratified_day;
                }
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return new Dictionary<string, int>(StringComparer.Ordinal);
                                }
            return ratification;
        }

        public static FoundryProductionFile LoadProduction(
            string dataDirectory,
            IFileIO files = null!,
            IJsonSerializer serializer = null!)
        {
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            string path = Path.Combine(dataDirectory, ProductionFileName);
            if (!files.FileExists(path)) return new FoundryProductionFile();
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return new FoundryProductionFile();
            try
            {
                return serializer.Deserialize<FoundryProductionFile>(text) ?? new FoundryProductionFile();
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return new FoundryProductionFile();
                                }
        }

        public static FoundryFactionEntry? LoadFaction(
            string dataDirectory,
            IFileIO files = null!,
            IJsonSerializer serializer = null!)
        {
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            string path = Path.Combine(dataDirectory, FactionFileName);
            if (!files.FileExists(path)) return null;
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return null;
            try
            {
                return serializer.Deserialize<FoundryFactionEntry>(text);
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return null;
                                }
        }
    }

    /// <summary>
    /// In-memory lookup surface over the static Foundry catalogs. Static data
    /// only — mutable simulation state lives in SilentFoundrySystem.
    /// </summary>
    public sealed class SilentFoundryCatalog
    {
        private readonly Dictionary<string, FoundryProductEntry> _byProductId =
            new Dictionary<string, FoundryProductEntry>(StringComparer.Ordinal);

        private readonly List<FoundryProductEntry> _products = new List<FoundryProductEntry>();

        public FoundryFactionEntry Faction { get; private set; }

        public IReadOnlyList<FoundryProductEntry> AllProducts => _products;
        public int ProductCount => _products.Count;

        public void Load(FoundryProductionFile production, FoundryFactionEntry faction)
        {
            _byProductId.Clear();
            _products.Clear();
            Faction = faction;

            if (production?.products == null) return;
            foreach (var p in production.products)
            {
                if (p == null || string.IsNullOrEmpty(p.product_id)) continue;
                if (!_byProductId.ContainsKey(p.product_id))
                {
                    _byProductId[p.product_id] = p;
                    _products.Add(p);
                }
            }
        }

        public FoundryProductEntry? GetProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId)) return null;
            return _byProductId.TryGetValue(productId, out var entry) ? entry : null;
        }

        public List<FoundryProductEntry> GetByCategory(string category)
        {
            var results = new List<FoundryProductEntry>();
            if (string.IsNullOrEmpty(category)) return results;
            for (int i = 0; i < _products.Count; i++)
                if (string.Equals(_products[i].category, category, StringComparison.OrdinalIgnoreCase))
                    results.Add(_products[i]);
            return results;
        }

        /// <summary>Products that carry a treaty obligation (quota_amount > 0).</summary>
        public List<FoundryProductEntry> GetQuotaProducts()
        {
            var results = new List<FoundryProductEntry>();
            for (int i = 0; i < _products.Count; i++)
                if (_products[i].quota_amount > 0 && !string.IsNullOrEmpty(_products[i].treaty_id))
                    results.Add(_products[i]);
            return results;
        }
    }
}
