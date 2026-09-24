// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Mods
{
    /// <summary>
    /// Plan 165 — strict loader for the authored mod-manifest specification.
    /// The lenient <see cref="ModSupportSystem.LoadSpecification"/> path remains
    /// for legacy callers; the host binds through this loader so a malformed
    /// specification is a loud load error rather than a silent fallback to the
    /// built-in whitelist (which could admit a catalog the author removed).
    /// </summary>
    public static class ModManifestSpecificationLoader
    {
        public const int CurrentSchemaVersion = 1;

        public static ModManifestSpecificationDef LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("mod_manifest_schema: specification JSON is empty.");

            ModManifestSpecificationDef? spec;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                spec = JsonSerializer.Deserialize<ModManifestSpecificationDef>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("mod_manifest_schema: malformed JSON (" + ex.Message + ").", ex);
            }

            if (spec == null)
                throw new InvalidOperationException("mod_manifest_schema: specification deserialized to null.");

            var errors = new List<string>();

            if (spec.SchemaVersion < 1 || spec.SchemaVersion > CurrentSchemaVersion)
                errors.Add($"unsupported schema_version {spec.SchemaVersion} (expected 1).");
            if (string.IsNullOrWhiteSpace(spec.SchemaName))
                errors.Add("schema_name is required.");
            if (spec.CurrentModContractVersion < 1)
                errors.Add($"current_mod_contract_version {spec.CurrentModContractVersion} must be >= 1.");
            if (spec.RequiredManifestFields == null || spec.RequiredManifestFields.Count == 0)
                errors.Add("required_manifest_fields must not be empty.");
            if (spec.AllowedCatalogs == null || spec.AllowedCatalogs.Count == 0)
                errors.Add("allowed_catalogs must not be empty.");

            var seenCatalogs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (spec.AllowedCatalogs != null)
            {
                for (int i = 0; i < spec.AllowedCatalogs.Count; i++)
                {
                    string catalog = spec.AllowedCatalogs[i];
                    if (string.IsNullOrWhiteSpace(catalog))
                        errors.Add($"allowed_catalogs[{i}] is empty.");
                    else if (!seenCatalogs.Add(catalog.Trim()))
                        errors.Add($"duplicate allowed catalog '{catalog}'.");
                }
            }

            var seenFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (spec.RequiredManifestFields != null)
            {
                for (int i = 0; i < spec.RequiredManifestFields.Count; i++)
                {
                    string field = spec.RequiredManifestFields[i];
                    if (string.IsNullOrWhiteSpace(field))
                        errors.Add($"required_manifest_fields[{i}] is empty.");
                    else if (!seenFields.Add(field.Trim()))
                        errors.Add($"duplicate required manifest field '{field}'.");
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException("mod_manifest_schema: " + string.Join(" ", errors));

            return spec;
        }
    }
}
