// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Ashfall.Core.Shelter
{
    /// <summary>Gameplay-safe operating profile for the abstract synthesis plant.</summary>
    [Serializable]
    public sealed class FischerTropschReactorProfile
    {
        public string reactor_profile_id = string.Empty;
        public string display_name = string.Empty;
        public string feedstock_item_id = string.Empty;
        public int feedstock_units = 1;
        public float base_conversion_units = 1f;
        public int process_ticks = 2;
        public float thermal_min = 0.4f;
        public float thermal_max = 0.75f;
        public float pressure_min = 0.45f;
        public float pressure_max = 0.70f;
        public float catalyst_decay_per_tick = 1f;
        public float off_band_yield_floor = 0.35f;
        public int maintenance_interval_ticks = 8;
        public string catalyst_profile_id = string.Empty;
        public string catalyst_item_id = string.Empty;
        public int catalyst_units = 1;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(reactor_profile_id)) { error = "reactor_profile_id empty"; return false; }
            if (string.IsNullOrWhiteSpace(feedstock_item_id) || feedstock_units <= 0)
            { error = $"Reactor '{reactor_profile_id}' needs positive feedstock input"; return false; }
            if (base_conversion_units <= 0f || float.IsNaN(base_conversion_units) || float.IsInfinity(base_conversion_units))
            { error = $"Reactor '{reactor_profile_id}' conversion invalid"; return false; }
            if (process_ticks <= 0 || maintenance_interval_ticks <= 0)
            { error = $"Reactor '{reactor_profile_id}' tick interval invalid"; return false; }
            if (thermal_min < 0f || thermal_max > 1f || thermal_min > thermal_max
                || pressure_min < 0f || pressure_max > 1f || pressure_min > pressure_max)
            { error = $"Reactor '{reactor_profile_id}' operating band invalid"; return false; }
            if (catalyst_decay_per_tick < 0f || off_band_yield_floor < 0f || off_band_yield_floor > 1f)
            { error = $"Reactor '{reactor_profile_id}' catalyst/yield bounds invalid"; return false; }
            if (string.IsNullOrWhiteSpace(catalyst_profile_id) || string.IsNullOrWhiteSpace(catalyst_item_id) || catalyst_units <= 0)
            { error = $"Reactor '{reactor_profile_id}' catalyst input invalid"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class FischerTropschProductProfile
    {
        public string product_id = string.Empty;
        public string item_id = string.Empty;
        public string grade = "standard";
        public string output_kind = "lubricant"; // lubricant | wax | light_fraction
        public float split = 0.5f;
        public float units_per_conversion = 1f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(product_id) || string.IsNullOrWhiteSpace(item_id))
            { error = "product id/item id empty"; return false; }
            if (split < 0f || split > 1f || units_per_conversion < 0f)
            { error = $"Product '{product_id}' output bounds invalid"; return false; }
            if (output_kind != "lubricant" && output_kind != "wax" && output_kind != "light_fraction")
            { error = $"Product '{product_id}' output_kind invalid"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class FischerTropschCatalystProfile
    {
        public string catalyst_profile_id = string.Empty;
        public float starting_condition = 100f;
        public float spent_threshold = 10f;
        public float replacement_condition = 100f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(catalyst_profile_id)) { error = "catalyst_profile_id empty"; return false; }
            if (starting_condition <= 0f || starting_condition > 100f || spent_threshold < 0f
                || spent_threshold >= starting_condition || replacement_condition <= 0f || replacement_condition > 100f)
            { error = $"Catalyst '{catalyst_profile_id}' condition bounds invalid"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class FischerTropschCatalogDto
    {
        public int schema_version = 1;
        public List<FischerTropschReactorProfile> reactor_profiles = new List<FischerTropschReactorProfile>();
        public List<FischerTropschProductProfile> products = new List<FischerTropschProductProfile>();
        public List<FischerTropschCatalystProfile> catalyst_profiles = new List<FischerTropschCatalystProfile>();
    }

    public sealed class FischerTropschCatalog
    {
        public int SchemaVersion { get; }
        public IReadOnlyList<FischerTropschReactorProfile> Reactors { get; }
        public IReadOnlyList<FischerTropschProductProfile> Products { get; }
        public IReadOnlyList<FischerTropschCatalystProfile> Catalysts { get; }

        public FischerTropschCatalog(FischerTropschCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            FischerTropschCatalogLoader.Validate(dto);
            SchemaVersion = dto.schema_version;
            Reactors = new List<FischerTropschReactorProfile>(dto.reactor_profiles);
            Products = new List<FischerTropschProductProfile>(dto.products);
            Catalysts = new List<FischerTropschCatalystProfile>(dto.catalyst_profiles);
        }

        public FischerTropschReactorProfile? FindReactor(string id)
        {
            foreach (var profile in Reactors)
                if (profile.reactor_profile_id == id) return profile;
            return null;
        }

        public FischerTropschCatalystProfile? FindCatalyst(string id)
        {
            foreach (var profile in Catalysts)
                if (profile.catalyst_profile_id == id) return profile;
            return null;
        }
    }

    public static class FischerTropschCatalogLoader
    {
        public const string CatalogFileName = "fischer_tropsch_catalog.json";

        public static FischerTropschCatalog Load(string dataDirectory, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDirectory, CatalogFileName);
            if (!fileIO.FileExists(path))
                throw new InvalidOperationException($"Missing {CatalogFileName}: {path}");
            var dto = serializer.Deserialize<FischerTropschCatalogDto>(fileIO.ReadAllText(path));
            if (dto == null) throw new InvalidOperationException($"{CatalogFileName} deserialized null");
            return new FischerTropschCatalog(dto);
        }

        public static void Validate(FischerTropschCatalogDto dto)
        {
            if (dto.schema_version < 1) throw new InvalidOperationException("fischer_tropsch_catalog schema_version must be >= 1");
            if (dto.reactor_profiles == null || dto.reactor_profiles.Count == 0)
                throw new InvalidOperationException("fischer_tropsch_catalog requires a reactor profile");
            if (dto.products == null || dto.products.Count == 0)
                throw new InvalidOperationException("fischer_tropsch_catalog requires products");
            if (dto.catalyst_profiles == null || dto.catalyst_profiles.Count == 0)
                throw new InvalidOperationException("fischer_tropsch_catalog requires catalysts");

            var reactorIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var profile in dto.reactor_profiles)
            {
                if (profile == null) throw new InvalidOperationException("Null reactor profile");
                string error;
                if (!profile.Validate(out error)) throw new InvalidOperationException(error);
                if (!reactorIds.Add(profile.reactor_profile_id)) throw new InvalidOperationException($"Duplicate reactor '{profile.reactor_profile_id}'");
            }

            var productIds = new HashSet<string>(StringComparer.Ordinal);
            var splitByKind = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var product in dto.products)
            {
                if (product == null) throw new InvalidOperationException("Null product profile");
                string error;
                if (!product.Validate(out error)) throw new InvalidOperationException(error);
                if (!productIds.Add(product.product_id)) throw new InvalidOperationException($"Duplicate product '{product.product_id}'");
                splitByKind.TryGetValue(product.output_kind, out float split);
                splitByKind[product.output_kind] = split + product.split;
            }
            foreach (var kind in new[] { "lubricant", "wax", "light_fraction" })
            {
                if (!splitByKind.TryGetValue(kind, out float split) || split <= 0f)
                    throw new InvalidOperationException($"No positive {kind} product split");
                if (split > 1.001f) throw new InvalidOperationException($"{kind} product split exceeds one");
            }

            var catalystIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var catalyst in dto.catalyst_profiles)
            {
                if (catalyst == null) throw new InvalidOperationException("Null catalyst profile");
                string error;
                if (!catalyst.Validate(out error)) throw new InvalidOperationException(error);
                if (!catalystIds.Add(catalyst.catalyst_profile_id)) throw new InvalidOperationException($"Duplicate catalyst '{catalyst.catalyst_profile_id}'");
            }
        }
    }
}
