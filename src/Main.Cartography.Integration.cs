// SPDX-License-Identifier: MIT
using Ashfall.Core.Exploration;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
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

    }
}
