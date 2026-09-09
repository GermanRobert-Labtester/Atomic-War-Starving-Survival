// SPDX-License-Identifier: MIT
// ASHFALL settlement codex host wiring.
//
// Read-only projection: the codex stores nothing of its own. Unlock state is
// derived from the journal's knowledge keys, the field guide's observations,
// the research engine's state and the Year of Ash faction-standing authority,
// all of which already persist themselves. That is why this partial has a
// SetupCodex() and no SaveCodex() twin — it is allowlisted in
// MainTriadDriftGateTests with that disposition, following the Enrichment
// precedent for static catalog projections.

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Codex;
using Ashfall.Core.IO;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CodexHostSession? _codex;
        private FieldGuideCatalog? _codexFieldGuide;

        public CodexHostSession? Codex
        {
            get { SetupCodex(); return _codex; }
        }

        private void SetupCodex()
        {
            if (_codex != null) return;
            _codex = CodexHostSession.Create(_dataDir, new GodotLog());
        }

        /// <summary>
        /// The field guide catalog, reusing the ecological-infestation instance
        /// when that system has already loaded one so observations unlocked by
        /// sighting are visible to the codex instead of being read twice.
        /// </summary>
        private FieldGuideCatalog? CodexFieldGuide()
        {
            if (_fieldGuide != null) return _fieldGuide;
            if (_codexFieldGuide == null)
                _codexFieldGuide = FieldGuideCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
            return _codexFieldGuide;
        }

        /// <summary>
        /// Faction contact for <c>meet_faction</c> codex unlocks: a faction the
        /// shelter holds any recorded standing with has been met. Standing is the
        /// Year of Ash faction-war authority, which already persists this, so the
        /// codex adds no second record of who has been encountered.
        /// </summary>
        private bool CodexFactionContact(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return (_yearOfAsh?.FactionWar.GetStanding(factionId) ?? 0f) != 0f;
        }

        private int CodexDay() => _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;

        /// <summary>
        /// Full codex projection: authored records plus field guide, research and
        /// journal sources. <paramref name="maxSpoilerTier"/> lets a surface
        /// withhold late revelations entirely rather than showing them locked.
        /// </summary>
        public IReadOnlyList<CodexEntryProjection> BuildCodexProjection(int maxSpoilerTier = int.MaxValue)
        {
            SetupCodex();
            SetupJournal();
            var research = EnsureSharedResearch();
            return _codex!.Build(
                CodexFieldGuide(),
                research?.State,
                research?.Catalog,
                _journal,
                CodexDay(),
                CodexFactionContact,
                maxSpoilerTier);
        }

        /// <summary>Codex records attached to one location, for place-scoped surfaces.</summary>
        public IReadOnlyList<CodexEntryProjection> BuildCodexForLocation(
            string locationId, int maxSpoilerTier = int.MaxValue)
        {
            SetupCodex();
            SetupJournal();
            return _codex!.BuildForLocation(locationId, _journal, CodexDay(), CodexFactionContact, maxSpoilerTier);
        }

        /// <summary>
        /// One-line recovery summary for a status strip: how many records the
        /// shelter has recovered, per discipline, without naming what is missing.
        /// </summary>
        public string CodexRecoverySummary(int maxSpoilerTier = int.MaxValue)
        {
            SetupCodex();
            SetupJournal();
            var research = EnsureSharedResearch();
            var counts = _codex!.KnownCountByCategory(
                CodexFieldGuide(), research?.State, research?.Catalog, _journal,
                CodexDay(), CodexFactionContact, maxSpoilerTier);

            int known = counts.Values.Sum();
            if (known == 0)
                return "Codex empty. Nothing has been surveyed, studied, or written down yet.";

            var parts = counts
                .Where(kv => kv.Value > 0)
                .OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Value} {CodexCategoryLabel(kv.Key)}");
            return $"Codex: {known} recovered — {string.Join(", ", parts)}.";
        }

        private static string CodexCategoryLabel(CodexCategory category)
        {
            switch (category)
            {
                case CodexCategory.Ecology: return "ecology";
                case CodexCategory.Technology: return "technology";
                case CodexCategory.WastelandLore: return "wasteland lore";
                case CodexCategory.SurvivalOperations: return "operations";
                case CodexCategory.Factions: return "factions";
                default: return category.ToString().ToLowerInvariant();
            }
        }
    }
}
