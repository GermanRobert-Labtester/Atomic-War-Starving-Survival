// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Underground
{
    /// <summary>
    /// Plan 167 — strict loader for the authored underground tunnel catalog.
    /// The lenient <see cref="TunnelNetworkSystem.LoadCatalog"/> path remains for
    /// legacy callers; the host only binds through this loader, so an authored
    /// typo is a loud load error instead of a silently dropped junction or
    /// segment (an unreachable-but-"declared" tunnel).
    /// </summary>
    public static class TunnelNetworkCatalogLoader
    {
        public const int CurrentSchemaVersion = 1;

        public static TunnelNetworkCatalogData LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("underground_tunnels: catalog JSON is empty.");

            TunnelNetworkCatalogData? catalog;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                catalog = JsonSerializer.Deserialize<TunnelNetworkCatalogData>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("underground_tunnels: malformed JSON (" + ex.Message + ").", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("underground_tunnels: catalog deserialized to null.");

            var errors = new List<string>();

            if (catalog.schema_version < 1 || catalog.schema_version > CurrentSchemaVersion)
                errors.Add($"unsupported schema_version {catalog.schema_version} (expected 1).");

            if (catalog.junctions == null || catalog.junctions.Count == 0)
                errors.Add("no junctions declared.");
            if (catalog.segments == null || catalog.segments.Count == 0)
                errors.Add("no segments declared.");

            var junctionIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (catalog.junctions != null)
            {
                for (int i = 0; i < catalog.junctions.Count; i++)
                {
                    var junction = catalog.junctions[i];
                    if (junction == null) { errors.Add($"junction {i} is null."); continue; }
                    if (string.IsNullOrWhiteSpace(junction.id))
                        errors.Add($"junction {i} has an empty id.");
                    else if (!junctionIds.Add(junction.id.Trim()))
                        errors.Add($"duplicate junction id '{junction.id}'.");
                    if (string.IsNullOrWhiteSpace(junction.name))
                        errors.Add($"junction '{junction.id}' has an empty name.");
                }
            }

            var segmentIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (catalog.segments != null)
            {
                for (int i = 0; i < catalog.segments.Count; i++)
                {
                    var segment = catalog.segments[i];
                    if (segment == null) { errors.Add($"segment {i} is null."); continue; }

                    if (string.IsNullOrWhiteSpace(segment.id))
                        errors.Add($"segment {i} has an empty id.");
                    else if (!segmentIds.Add(segment.id.Trim()))
                        errors.Add($"duplicate segment id '{segment.id}'.");

                    if (string.IsNullOrWhiteSpace(segment.name))
                        errors.Add($"segment '{segment.id}' has an empty name.");
                    if (string.IsNullOrWhiteSpace(segment.from) || string.IsNullOrWhiteSpace(segment.to))
                        errors.Add($"segment '{segment.id}' is missing an endpoint.");
                    if (string.Equals(segment.from, segment.to, StringComparison.OrdinalIgnoreCase))
                        errors.Add($"segment '{segment.id}' connects a node to itself.");

                    if (float.IsNaN(segment.length_hours) || float.IsInfinity(segment.length_hours) || segment.length_hours < 0.5f)
                        errors.Add($"segment '{segment.id}' has length_hours {segment.length_hours} (minimum 0.5).");
                    if (segment.difficulty < 1 || segment.difficulty > 5)
                        errors.Add($"segment '{segment.id}' has difficulty {segment.difficulty} (expected 1..5).");
                    if (float.IsNaN(segment.integrity) || segment.integrity < 0f || segment.integrity > 100f)
                        errors.Add($"segment '{segment.id}' has integrity {segment.integrity} (expected 0..100).");

                    if (segment.hazards != null)
                    {
                        for (int h = 0; h < segment.hazards.Count; h++)
                        {
                            if (!Enum.TryParse<TunnelHazardType>(segment.hazards[h], true, out _))
                                errors.Add($"segment '{segment.id}' declares unknown hazard '{segment.hazards[h]}'.");
                        }
                    }
                }
            }

            // A junction that references a segment which does not exist is an
            // authoring error; validate the reference rather than letting it be
            // silently dropped at load time.
            if (catalog.junctions != null)
            {
                for (int i = 0; i < catalog.junctions.Count; i++)
                {
                    var junction = catalog.junctions[i];
                    if (junction?.connected_segments == null) continue;
                    for (int c = 0; c < junction.connected_segments.Count; c++)
                    {
                        string segmentId = junction.connected_segments[c];
                        if (string.IsNullOrWhiteSpace(segmentId) || !segmentIds.Contains(segmentId.Trim()))
                            errors.Add($"junction '{junction.id}' references unknown segment '{segmentId}'.");
                    }
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException("underground_tunnels: " + string.Join(" ", errors));

            return catalog;
        }
    }
}
