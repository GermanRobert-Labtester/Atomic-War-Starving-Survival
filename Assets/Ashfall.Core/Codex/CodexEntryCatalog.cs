// SPDX-License-Identifier: MIT
// ASHFALL Core: authored codex entry catalog (codex_entries.json).

using System;
using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Codex
{
    /// <summary>How an authored codex entry becomes known to the shelter.</summary>
    public enum CodexUnlockCondition
    {
        /// <summary>Unrecognized condition: never unlocks rather than unlocking by accident.</summary>
        Unknown = 0,
        VisitLocation = 1,
        MeetFaction = 2,
        FirstCatch = 3
    }

    /// <summary>
    /// One authored codex record from <c>codex_entries.json</c>: regional, location,
    /// faction, deep-lore and wildlife prose that the settlement can recover by
    /// travelling, making contact, or hunting. Authored content only — this type
    /// carries no runtime state; unlock state is derived by
    /// <see cref="CodexProjectionBuilder"/> from the journal and host authorities.
    /// </summary>
    public sealed class AuthoredCodexEntry
    {
        public string id = string.Empty;
        public string category = string.Empty;
        public string displayName = string.Empty;
        public int spoilerTier;
        public CodexUnlockCondition unlockCondition = CodexUnlockCondition.Unknown;
        public string unlockRef = string.Empty;
        public string body = string.Empty;
        public string provenance = string.Empty;
        public List<string> tags = new List<string>();
    }

    /// <summary>
    /// Engine-agnostic loader for codex_entries.json. Missing file, malformed JSON
    /// and future schema all yield an empty list rather than throwing or parsing
    /// partially; entries without an id or without prose are skipped, and duplicate
    /// ids keep the first definition so the catalog can never hold two records
    /// under one id.
    /// </summary>
    public static class CodexEntryCatalogLoader
    {
        public const string FileName = "codex_entries.json";
        public const int CurrentSchemaVersion = 1;

        public const string CategoryRegions = "regions";
        public const string CategoryLocations = "locations";
        public const string CategoryFactions = "factions";
        public const string CategoryDeepLore = "deep_lore";
        public const string CategoryWildlife = "wildlife";

        public const string ProvenanceCanonical = "canonical";
        public const string ProvenanceRestricted = "restricted";
        public const string ProvenanceEyewitness = "eyewitness";
        public const string ProvenanceMaterial = "material";
        public const string ProvenanceRumor = "rumor";

        public static List<AuthoredCodexEntry> LoadEntries(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<AuthoredCodexEntry>();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return new List<AuthoredCodexEntry>();

            string raw = fileIO.ReadAllText(path);
            return Parse(raw, json, path);
        }

        /// <summary>Parse an already-read payload. Exposed for tests and host tooling.</summary>
        public static List<AuthoredCodexEntry> Parse(
            string raw, IJsonSerializer json, string? pathForDiagnostics = null)
        {
            var result = new List<AuthoredCodexEntry>();
            if (json == null || string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var root = json.Deserialize<CodexRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion) return result;
                if (root.entries == null) return result;

                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < root.entries.Count; i++)
                {
                    var e = root.entries[i];
                    if (e == null) continue;
                    if (string.IsNullOrEmpty(e.id) || string.IsNullOrEmpty(e.body)) continue;
                    if (!seen.Add(e.id)) continue;

                    var entry = new AuthoredCodexEntry
                    {
                        id = e.id,
                        category = e.category ?? string.Empty,
                        displayName = string.IsNullOrEmpty(e.display_name) ? e.id : e.display_name!,
                        spoilerTier = e.spoiler_tier,
                        unlockCondition = ParseUnlockCondition(e.unlock_condition),
                        unlockRef = e.unlock_ref ?? string.Empty,
                        body = e.body,
                        provenance = e.provenance ?? string.Empty
                    };

                    if (e.tags != null)
                    {
                        for (int t = 0; t < e.tags.Count; t++)
                        {
                            var tag = e.tags[t];
                            if (!string.IsNullOrEmpty(tag) && !entry.tags.Contains(tag))
                                entry.tags.Add(tag);
                        }
                    }

                    result.Add(entry);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(
                    pathForDiagnostics ?? FileName, "CodexEntryRoot", ex_CATDIAG);
                return result;
            }

            return result;
        }

        /// <summary>
        /// Maps the authored condition string. An unrecognized string becomes
        /// <see cref="CodexUnlockCondition.Unknown"/>, which never unlocks — a
        /// typo in the data must not silently reveal spoiler-tiered lore.
        /// </summary>
        public static CodexUnlockCondition ParseUnlockCondition(string? condition)
        {
            if (string.IsNullOrEmpty(condition)) return CodexUnlockCondition.Unknown;
            if (string.Equals(condition, "visit_location", StringComparison.Ordinal)) return CodexUnlockCondition.VisitLocation;
            if (string.Equals(condition, "meet_faction", StringComparison.Ordinal)) return CodexUnlockCondition.MeetFaction;
            if (string.Equals(condition, "first_catch", StringComparison.Ordinal)) return CodexUnlockCondition.FirstCatch;
            return CodexUnlockCondition.Unknown;
        }

        private class CodexRoot
        {
            public int schema_version = 1;
            public string collection_id;
            public List<Entry> entries = new List<Entry>();
        }

        private class Entry
        {
            public string id;
            public string category;
            public string display_name;
            public int spoiler_tier;
            public string unlock_condition;
            public string unlock_ref;
            public string body;
            public string provenance;
            public List<string> tags;
        }
    }
}
