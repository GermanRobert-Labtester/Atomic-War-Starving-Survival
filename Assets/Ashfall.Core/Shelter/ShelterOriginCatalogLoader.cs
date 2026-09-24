// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Plan 166 — strict loader for the authored shelter-origins catalog.
    /// The lenient <see cref="ShelterIdentitySystem.LoadCatalog"/> path remains
    /// for legacy callers; the host binds through this loader so a malformed
    /// origin is a loud load error rather than a silently absent origin.
    /// </summary>
    public static class ShelterOriginCatalogLoader
    {
        public const int CurrentSchemaVersion = 1;
        private const int MaxBasisPoints = 5000;

        public static ShelterOriginsCatalogJson LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("shelter_origins: catalog JSON is empty.");

            ShelterOriginsCatalogJson? catalog;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
                catalog = JsonSerializer.Deserialize<ShelterOriginsCatalogJson>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("shelter_origins: malformed JSON (" + ex.Message + ").", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("shelter_origins: catalog deserialized to null.");

            var errors = new List<string>();

            if (catalog.schema_version < 1 || catalog.schema_version > CurrentSchemaVersion)
                errors.Add($"unsupported schema_version {catalog.schema_version} (expected 1).");
            if (catalog.origins == null || catalog.origins.Count == 0)
                errors.Add("no origins declared.");

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (catalog.origins != null)
            {
                for (int i = 0; i < catalog.origins.Count; i++)
                {
                    var origin = catalog.origins[i];
                    if (origin == null) { errors.Add($"origin {i} is null."); continue; }

                    if (string.IsNullOrWhiteSpace(origin.origin_id))
                        errors.Add($"origin {i} has an empty origin_id.");
                    else if (!ids.Add(origin.origin_id.Trim()))
                        errors.Add($"duplicate origin_id '{origin.origin_id}'.");
                    if (string.IsNullOrWhiteSpace(origin.display_name))
                        errors.Add($"origin '{origin.origin_id}' has an empty display_name.");

                    CheckBasisPoints(errors, origin.origin_id, "radiation_shielding_bp", origin.radiation_shielding_bp);
                    CheckBasisPoints(errors, origin.origin_id, "ventilation_bonus_bp", origin.ventilation_bonus_bp);
                    CheckBasisPoints(errors, origin.origin_id, "space_modifier_bp", origin.space_modifier_bp);

                    if (origin.starting_bonuses == null || origin.starting_bonuses.Count == 0)
                        errors.Add($"origin '{origin.origin_id}' declares no starting bonuses.");
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException("shelter_origins: " + string.Join(" ", errors));

            return catalog;
        }

        private static void CheckBasisPoints(List<string> errors, string originId, string field, int value)
        {
            if (value < -MaxBasisPoints || value > MaxBasisPoints)
                errors.Add($"origin '{originId}' has {field} {value} (expected -{MaxBasisPoints}..{MaxBasisPoints}).");
        }
    }
}
