// SPDX-License-Identifier: MIT
// ============================================================================
// Authority : Core catalog entry DTOs + authored JSON (single source of truth)
// This file : host-side adapter ONLY — reads through the IFileIO port and
//             flattens the Core DTOs into presentational lines.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// One flattened line of authored process-log prose, ready for a panel.
    /// Values are read verbatim from the authored JSON — nothing is templated,
    /// renamed, or synthesised here.
    /// </summary>
    public sealed class AssayLogLine
    {
        public string EntryId { get; }
        public string Kind { get; }
        public string TimestampRelative { get; }
        public string Prose { get; }
        public IReadOnlyList<string> Tags { get; }

        public AssayLogLine(string entryId, string kind, string timestampRelative, string prose, IReadOnlyList<string>? tags)
        {
            EntryId = entryId ?? string.Empty;
            Kind = kind ?? string.Empty;
            TimestampRelative = timestampRelative ?? string.Empty;
            Prose = prose ?? string.Empty;
            Tags = tags ?? Array.Empty<string>();
        }

        public string Label => string.IsNullOrEmpty(EntryId) ? Kind : EntryId;
    }

    /// <summary>
    /// Maps one authored JSON file to one Core entry DTO. <see cref="Project"/>
    /// is the only place a specific DTO is named, so a new catalog needs no new
    /// loader — only a new spec.
    /// </summary>
    public sealed class AssayLogCatalogSpec
    {
        public string FileName { get; }
        public string Kind { get; }
        public Func<string, List<AssayLogLine>> Project { get; }

        public AssayLogCatalogSpec(string fileName, string kind, Func<string, List<AssayLogLine>> project)
        {
            FileName = fileName ?? string.Empty;
            Kind = kind ?? string.Empty;
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }

    /// <summary>
    /// W2-06 · Decision Point 1 · Path B — "revive with existing consumers".
    ///
    /// The authored process-log corpora (fermentation assays, water-treatment
    /// titration reports, cartography hazard sheets, glassworks aging logs)
    /// had no runtime consumer. Their Core convenience wrappers expose only
    /// <c>LoadFromDirectory(string)</c> backed by <c>System.IO.File.ReadAllText</c>,
    /// which returns nothing when the Data directory lives inside the .pck
    /// (<c>CatalogPath.ResolveDataDir()</c> can yield a <c>res://</c> path).
    /// Wiring those wrappers would compile, pass in the editor, and be silently
    /// empty in an exported build.
    ///
    /// This adapter instead reads through the existing <see cref="IFileIO"/>
    /// port — the same port every live catalog load uses — parses with the
    /// Core's own <c>CatalogLocator.LoadWrappedList&lt;T&gt;</c>, and projects the
    /// Core's own public entry DTOs. No schema, authority, or data is duplicated,
    /// and a file that fails to read is reported rather than being silently
    /// dropped.
    ///
    /// Output is deterministic: spec order, then authored order, then an
    /// ordinal sort by entry id.
    /// </summary>
    public static class NarrativeAssayLogCatalogLoader
    {
        public const string SubDirectory = "narrative";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        public static List<AssayLogLine> Load(
            string? dataDir,
            IFileIO? io,
            Ashfall.Core.IJsonSerializer? serializer,
            IReadOnlyList<AssayLogCatalogSpec>? specs,
            List<string>? problems = null)
        {
            var result = new List<AssayLogLine>();
            if (string.IsNullOrWhiteSpace(dataDir) || io == null || serializer == null || specs == null || specs.Count == 0)
                return result;

            foreach (var spec in specs)
            {
                string path = io.Combine(dataDir, SubDirectory, spec.FileName);
                if (!io.FileExists(path)) continue;

                try
                {
                    string json = io.ReadAllText(path);
                    List<AssayLogLine> lines = spec.Project(json);
                    if (lines == null) continue;
                    foreach (var line in lines)
                        if (line != null && !string.IsNullOrWhiteSpace(line.Prose))
                            result.Add(line);
                }
                catch (Exception ex)
                {
                    Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "NarrativeAssayLogCatalog", ex);
                    problems?.Add(spec.FileName + ": " + ex.Message);
                }
            }

            result.Sort((a, b) => string.CompareOrdinal(a.EntryId, b.EntryId));
            return result;
        }

        internal static JsonSerializerOptions SerializerOptions => Options;
    }
}
