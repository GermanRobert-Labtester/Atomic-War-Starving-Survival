// SPDX-License-Identifier: MIT
// ASHFALL settlement codex host session — composes the read-only projection.

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Codex;
using Ashfall.Core.Journal;
using Ashfall.Core.Research;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side owner of the settlement codex. Loads the authored records from
    /// <c>codex_entries.json</c> and composes them with the field guide, research
    /// and journal authorities into <see cref="CodexProjectionBuilder"/>'s
    /// read-only projection.
    /// <para>
    /// There is deliberately no save store and no Main save triad here: the
    /// codex is a pure projection with zero persistent state, deriving every
    /// fact on demand from systems that already persist their own. Unlock state
    /// therefore survives save/load for free, because it is recomputed from the
    /// journal's knowledge keys rather than stored twice.
    /// </para>
    /// </summary>
    public sealed class CodexHostSession
    {
        private readonly List<AuthoredCodexEntry> _authored;

        public CodexHostSession(IReadOnlyList<AuthoredCodexEntry>? authored)
        {
            _authored = authored != null
                ? new List<AuthoredCodexEntry>(authored)
                : new List<AuthoredCodexEntry>();
        }

        public static CodexHostSession Create(string dataDir, ILog? log = null)
        {
            var entries = CodexEntryCatalogLoader.LoadEntries(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            if (log != null)
                log.Info($"[codex] loaded {entries.Count} authored codex records");
            return new CodexHostSession(entries);
        }

        /// <summary>Authored record count loaded from the data authority.</summary>
        public int AuthoredEntryCount => _authored.Count;

        public IReadOnlyList<AuthoredCodexEntry> AuthoredEntries => _authored;

        /// <summary>
        /// Builds the full codex projection from the supplied authorities. Any
        /// authority may be null; the corresponding source simply contributes
        /// nothing, which keeps the projection usable early in a campaign.
        /// </summary>
        public IReadOnlyList<CodexEntryProjection> Build(
            FieldGuideCatalog? fieldGuide,
            ResearchState? researchState,
            IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog,
            JournalSystem? journal,
            int currentDay,
            Func<string, bool>? factionContact = null,
            int maxSpoilerTier = int.MaxValue)
        {
            return CodexProjectionBuilder.Build(
                fieldGuide,
                researchState,
                researchCatalog,
                journal,
                currentDay,
                _authored,
                factionContact,
                maxSpoilerTier);
        }

        /// <summary>Only the records the shelter has actually recovered.</summary>
        public IReadOnlyList<CodexEntryProjection> BuildKnown(
            FieldGuideCatalog? fieldGuide,
            ResearchState? researchState,
            IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog,
            JournalSystem? journal,
            int currentDay,
            Func<string, bool>? factionContact = null,
            int maxSpoilerTier = int.MaxValue)
        {
            return Build(fieldGuide, researchState, researchCatalog, journal,
                    currentDay, factionContact, maxSpoilerTier)
                .Where(e => e.State == CodexEntryState.Known)
                .ToList();
        }

        /// <summary>
        /// Authored records projected for one location, for surfaces that are
        /// already showing a place: the location's own record plus the region
        /// record it belongs to, recovered ones first.
        /// </summary>
        public IReadOnlyList<CodexEntryProjection> BuildForLocation(
            string locationId,
            JournalSystem? journal,
            int currentDay,
            Func<string, bool>? factionContact = null,
            int maxSpoilerTier = int.MaxValue)
        {
            if (string.IsNullOrEmpty(locationId)) return Array.Empty<CodexEntryProjection>();
            return Build(null, null, null, journal, currentDay, factionContact, maxSpoilerTier)
                .Where(e => e.RelatedLocationIds.Contains(locationId))
                .ToList();
        }

        /// <summary>Recovered-record count per category, for a summary strip.</summary>
        public IReadOnlyDictionary<CodexCategory, int> KnownCountByCategory(
            FieldGuideCatalog? fieldGuide,
            ResearchState? researchState,
            IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog,
            JournalSystem? journal,
            int currentDay,
            Func<string, bool>? factionContact = null,
            int maxSpoilerTier = int.MaxValue)
        {
            var counts = new Dictionary<CodexCategory, int>();
            foreach (CodexCategory c in Enum.GetValues(typeof(CodexCategory))) counts[c] = 0;
            foreach (var e in Build(fieldGuide, researchState, researchCatalog, journal,
                         currentDay, factionContact, maxSpoilerTier))
            {
                if (e.State == CodexEntryState.Known) counts[e.Category]++;
            }
            return counts;
        }
    }
}
