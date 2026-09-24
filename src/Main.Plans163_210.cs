// SPDX-License-Identifier: MIT
// Plan 163 + Plan 210: bounded production seams over canonical owners.
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Exploration;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>
        /// Read-only cartography projection over the world map's persisted
        /// knowledge. No region graph or discovery state is duplicated here.
        /// </summary>
        public IReadOnlyList<CanonicalMapSurvey> GetCartographyProjection()
        {
            SetupWorld();
            return _world?.WastelandMap == null
                ? Array.Empty<CanonicalMapSurvey>()
                : CartographySystem.ProjectCanonicalMap(
                    _world.WastelandMap.Nodes,
                    _world.WastelandMap.Knowledge);
        }

        /// <summary>
        /// Records a field survey through the canonical map owner and awards
        /// action XP to the existing scavenging discipline. The map save and
        /// skill save owners remain responsible for persistence.
        /// </summary>
        public bool RecordCartographySurvey(
            string nodeId,
            string surveyorId,
            IEnumerable<string>? traits = null)
        {
            if (string.IsNullOrWhiteSpace(nodeId) || string.IsNullOrWhiteSpace(surveyorId))
                return false;

            SetupWorld();
            SetupSurvivors();
            var survivor = _survivors?.Needs.Get(surveyorId);
            if (survivor == null || !survivor.IsAliveState || _world?.WastelandMap == null)
                return false;

            if (!_world.WastelandMap.DiscoverSurvey(nodeId.Trim(), surveyorId.Trim(), _simDay, traits))
                return false;

            // Cartography has no independent skill ledger. Field work is
            // authored as scavenging practice until a dedicated discipline is
            // present in the shared skills catalog.
            var skills = EnsureSharedSkillProgression();
            var actor = new SimpleSkillActor(surveyorId.Trim());
            skills.RecordAction(
                actor,
                "scavenging",
                xpAmount: 5f,
                currentDay: _simDay,
                rng: _campaignDay?.Rng.Fork(CampaignStreamIds.Expedition, _simDay, 163));

            _worldDirty = true;
            _mapPanel?.RefreshView();
            return true;
        }

        /// <summary>Returns one survivor's stable claim metadata.</summary>
        public IReadOnlyList<PersonalBelonging> GetPersonalBelongings(string survivorId)
        {
            SetupSurvivorSocial();
            return _survivorSocial?.Belongings.GetBelongingsForSurvivor(survivorId)
                ?? Array.Empty<PersonalBelonging>();
        }

        /// <summary>
        /// Claims an existing item definition as a survivor keepsake. Inventory
        /// remains the physical stack authority; this records only the stable
        /// survivor-to-item association and sentimental metadata.
        /// </summary>
        public bool ClaimPersonalBelonging(
            string survivorId,
            string itemId,
            BelongingCategory category = BelongingCategory.Keepsake,
            float sentimentalValue = 50f,
            string description = "")
        {
            return EnsurePersonalBelongings().Claim(survivorId, itemId, category, sentimentalValue, description);
        }

        /// <summary>Transfers claim metadata; physical inventory remains shared.</summary>
        public bool GiftPersonalBelonging(
            string fromSurvivorId,
            string toSurvivorId,
            string belongingId,
            string reason = "Gift")
        {
            return EnsurePersonalBelongings().Gift(fromSurvivorId, toSurvivorId, belongingId, reason);
        }

        public bool SetPersonalBelongingFavorite(
            string survivorId,
            string belongingId,
            bool isFavorite = true)
        {
            return EnsurePersonalBelongings().SetFavorite(survivorId, belongingId, isFavorite);
        }

        public bool ReportPersonalBelongingLoss(
            string survivorId,
            string belongingId,
            bool stolen = false)
        {
            return EnsurePersonalBelongings().ReportLoss(survivorId, belongingId, stolen);
        }

        /// <summary>
        /// Called exactly once by SurvivorFateSystem for each death. The
        /// deterministic roster order supplies a primary heir when one exists;
        /// no inventory item is duplicated or removed.
        /// </summary>
        private void HandlePersonalBelongingsInheritance(SurvivorFateEvent fate)
        {
            if (fate == null || string.IsNullOrEmpty(fate.survivorId)) return;
            SetupSurvivorSocial();
            SetupSurvivors();
            string heir = _survivors?.RosterState
                .Where(s => s != null && s.IsAliveState && !string.Equals(s.Id, fate.survivorId, StringComparison.Ordinal))
                .Select(s => s.Id)
                .FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(heir)) return;

            _survivorSocial!.Belongings.DistributeInheritanceOnDeath(
                fate.survivorId,
                heir,
                fate.day > 0 ? fate.day : _simDay);
        }
    }
}
