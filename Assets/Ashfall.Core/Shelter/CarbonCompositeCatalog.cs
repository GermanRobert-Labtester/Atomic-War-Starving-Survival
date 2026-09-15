// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class CompositeMaterialProfile
    {
        public string material_profile_id = string.Empty;
        public int freshness_days = 20;
        public float aging_penalty_per_day = 0.04f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(material_profile_id)) { error = "Composite material id is empty"; return false; }
            if (freshness_days <= 0 || aging_penalty_per_day < 0f || aging_penalty_per_day > 1f)
            { error = $"Composite material '{material_profile_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class CompositeComponentProfile
    {
        public string component_id = string.Empty;
        public string input_material_id = string.Empty;
        public int input_quantity = 1;
        public string output_item_id = string.Empty;
        public float mass_factor = 1f;
        public float durability_factor = 1f;
        public string required_quality = "field";
        public string cure_profile_id = string.Empty;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(component_id) || string.IsNullOrWhiteSpace(input_material_id)
                || string.IsNullOrWhiteSpace(output_item_id) || string.IsNullOrWhiteSpace(cure_profile_id))
            { error = "Composite component ids are incomplete"; return false; }
            if (input_quantity <= 0 || mass_factor <= 0f || mass_factor > 1.5f || durability_factor <= 0f || durability_factor > 2f)
            { error = $"Composite component '{component_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class CompositeCureProfile
    {
        public string cure_profile_id = string.Empty;
        public int process_ticks = 1;
        public float target_thermal = 0.7f;
        public float target_pressure = 0.65f;
        public float minimum_seal = 0.75f;
        public float defect_rate = 0.12f;

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(cure_profile_id)) { error = "Composite cure id is empty"; return false; }
            if (process_ticks <= 0 || target_thermal < 0f || target_thermal > 1f || target_pressure < 0f || target_pressure > 1f
                || minimum_seal < 0f || minimum_seal > 1f || defect_rate < 0f || defect_rate > 1f)
            { error = $"Composite cure '{cure_profile_id}' has invalid bounds"; return false; }
            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class CarbonCompositeCatalogDto
    {
        public int schema_version = 1;
        public List<CompositeMaterialProfile> materials = new List<CompositeMaterialProfile>();
        public List<CompositeComponentProfile> components = new List<CompositeComponentProfile>();
        public List<CompositeCureProfile> cures = new List<CompositeCureProfile>();
    }

    public sealed class CarbonCompositeCatalog
    {
        public int SchemaVersion { get; }
        public IReadOnlyList<CompositeMaterialProfile> Materials { get; }
        public IReadOnlyList<CompositeComponentProfile> Components { get; }
        public IReadOnlyList<CompositeCureProfile> Cures { get; }

        public CarbonCompositeCatalog(CarbonCompositeCatalogDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            CarbonCompositeCatalogLoader.Validate(dto);
            SchemaVersion = dto.schema_version;
            Materials = new List<CompositeMaterialProfile>(dto.materials);
            Components = new List<CompositeComponentProfile>(dto.components);
            Cures = new List<CompositeCureProfile>(dto.cures);
        }

        public CompositeMaterialProfile? FindMaterial(string id)
        {
            foreach (var profile in Materials) if (profile.material_profile_id == id) return profile;
            return null;
        }

        public CompositeComponentProfile? FindComponent(string id)
        {
            foreach (var profile in Components) if (profile.component_id == id) return profile;
            return null;
        }

        public CompositeCureProfile? FindCure(string id)
        {
            foreach (var profile in Cures) if (profile.cure_profile_id == id) return profile;
            return null;
        }
    }

    public static class CarbonCompositeCatalogLoader
    {
        public const string CatalogFileName = "carbon_composite_catalog.json";

        public static CarbonCompositeCatalog Load(string dataDirectory, IFileIO? fileIO = null, IJsonSerializer? serializer = null)
        {
            fileIO ??= new FileSystemIO();
            serializer ??= new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDirectory, CatalogFileName);
            if (!fileIO.FileExists(path)) throw new InvalidOperationException($"Missing {CatalogFileName}: {path}");
            var dto = serializer.Deserialize<CarbonCompositeCatalogDto>(fileIO.ReadAllText(path));
            if (dto == null) throw new InvalidOperationException($"{CatalogFileName} deserialized null");
            return new CarbonCompositeCatalog(dto);
        }

        public static void Validate(CarbonCompositeCatalogDto dto)
        {
            if (dto.schema_version < 1) throw new InvalidOperationException("carbon_composite_catalog schema_version must be >= 1");
            if (dto.materials == null || dto.materials.Count == 0 || dto.components == null || dto.components.Count == 0 || dto.cures == null || dto.cures.Count == 0)
                throw new InvalidOperationException("Composite catalog requires materials, components, and cures");
            var materials = new HashSet<string>(StringComparer.Ordinal);
            foreach (var material in dto.materials)
            {
                if (material == null) throw new InvalidOperationException("Null composite material");
                if (!material.Validate(out string error)) throw new InvalidOperationException(error);
                if (!materials.Add(material.material_profile_id)) throw new InvalidOperationException($"Duplicate composite material '{material.material_profile_id}'");
            }
            var components = new HashSet<string>(StringComparer.Ordinal);
            foreach (var component in dto.components)
            {
                if (component == null) throw new InvalidOperationException("Null composite component");
                if (!component.Validate(out string error)) throw new InvalidOperationException(error);
                if (!components.Add(component.component_id)) throw new InvalidOperationException($"Duplicate composite component '{component.component_id}'");
            }
            var cures = new HashSet<string>(StringComparer.Ordinal);
            foreach (var cure in dto.cures)
            {
                if (cure == null) throw new InvalidOperationException("Null composite cure");
                if (!cure.Validate(out string error)) throw new InvalidOperationException(error);
                if (!cures.Add(cure.cure_profile_id)) throw new InvalidOperationException($"Duplicate composite cure '{cure.cure_profile_id}'");
            }
            foreach (var component in dto.components)
            {
                if (!materials.Contains(component.input_material_id)) throw new InvalidOperationException($"Composite component '{component.component_id}' references unknown material '{component.input_material_id}'");
                if (!cures.Contains(component.cure_profile_id)) throw new InvalidOperationException($"Composite component '{component.component_id}' references unknown cure '{component.cure_profile_id}'");
            }
        }
    }
}
