// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PowerLoadSheddingHostSession
// Purpose      : EXPANSION-21-THE-GRID — microgrid load shedding, brownout
//                risk, and cascading trip evaluation.
//                Reads the live canonical owners and projects the sealed Core
//                engine's verdict. DERIVED READ MODEL: no save section, no
//                second power ledger, no breaker mutation (opening a breaker
//                stays PowerDistributionSubgridSystem.SetBreaker's command).
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Immutable projection of one load-shedding evaluation.</summary>
    public sealed class PowerLoadSheddingProjection
    {
        public int AvailableGenerationKw;
        public int TotalDemandKw;
        public int ServedLoadKw;
        public int ShedLoadKw;
        public int BrownoutRiskPermille;
        public int CascadingTripRiskPermille;
        public int EnergyPovertyMoralePenaltyPermille;
        public bool IsBlackout;
        public int DemandCount;
        public List<string> ShedConsumers = new();
        public int GridWearPermille;
    }

    /// <summary>
    /// Binds the sealed <see cref="PowerLoadSheddingEngine"/> to the live
    /// shelter power owners. Demand comes from the authored subgrid node
    /// definitions held by <see cref="PowerDistributionSubgridSystem"/>; supply
    /// and wear come from the live <see cref="PowerGridSystem"/>.
    /// </summary>
    public sealed class PowerLoadSheddingHostSession
    {
        private PowerGridSystem? _grid;
        private PowerDistributionSubgridSystem? _subgrids;

        public PowerGridSystem? Grid => _grid;
        public PowerDistributionSubgridSystem? Subgrids => _subgrids;

        /// <summary>Morale points a full (1000 permille) energy-poverty penalty costs per survivor per day.</summary>
        public const float MoralePointsAtFullPenalty = 6.0f;

        public PowerLoadSheddingHostSession(PowerGridSystem? grid = null, PowerDistributionSubgridSystem? subgrids = null)
        {
            _grid = grid;
            _subgrids = subgrids;
        }

        public void Bind(PowerGridSystem? grid, PowerDistributionSubgridSystem? subgrids)
        {
            _grid = grid;
            _subgrids = subgrids;
        }

        public bool IsBound => _grid != null;

        /// <summary>
        /// Projects the live grid wear (permille) from the canonical generator
        /// condition owner. Never stored here.
        /// </summary>
        public int GetGridWearPermille()
        {
            if (_grid == null) return 0;
            float condition = Math.Clamp(_grid.GeneratorCondition, 0f, 1f);
            int wear = (int)Math.Round((1.0 - condition) * PowerLoadSheddingEngine.PermilleScale);
            return Math.Max(0, Math.Min(PowerLoadSheddingEngine.PermilleScale, wear));
        }

        /// <summary>
        /// Builds the live demand vector: one entry per delivering subgrid node.
        /// Priority is the node's own criticality, falling back to the live
        /// room priority the power-grid owner reports. Disabled rooms demand
        /// nothing — they are intentionally off.
        /// </summary>
        public List<SubgridLoadDemand> BuildDemands()
        {
            var demands = new List<SubgridLoadDemand>();
            if (_subgrids == null) return demands;

            foreach (var def in _subgrids.NodeDefinitions)
            {
                if (def == null || string.IsNullOrEmpty(def.node_id)) continue;
                if (_subgrids.FindNode(def.node_id) is not { } node) continue;
                if (!node.is_breaker_closed || node.is_fuse_blown) continue; // not drawing power

                int kw = (int)Math.Round(Math.Max(0f, def.max_capacity_watts) / 1000f);
                if (kw <= 0) continue;

                demands.Add(new SubgridLoadDemand(def.node_id, ResolvePriority(def, node.target_room_id), kw));
            }

            return demands;
        }

        private LoadPriorityTier ResolvePriority(PowerSubgridNodeDefinition def, string roomId)
        {
            if (def.is_critical) return LoadPriorityTier.Tier0_LifeSupport;

            if (_grid != null && !string.IsNullOrEmpty(roomId))
            {
                switch (_grid.EffectivePriority(roomId))
                {
                    case PowerGridRoomPriority.Critical: return LoadPriorityTier.Tier1_Clinical;
                    case PowerGridRoomPriority.Standard: return LoadPriorityTier.Tier2_Agricultural;
                    case PowerGridRoomPriority.Low: return LoadPriorityTier.Tier4_Comfort;
                    case PowerGridRoomPriority.Disabled: return LoadPriorityTier.Tier4_Comfort;
                }
            }

            return LoadPriorityTier.Tier3_Industrial;
        }

        /// <summary>Runs the engine over the live owners and returns its verdict.</summary>
        public LoadSheddingEvaluationResult Evaluate() =>
            PowerLoadSheddingEngine.Evaluate(
                GetAvailableGenerationKw(),
                BuildDemands(),
                GetGridWearPermille());

        public int GetAvailableGenerationKw()
        {
            if (_grid == null) return 0;
            return (int)Math.Round(Math.Max(0f, _grid.AvailableSupplyWatts) / 1000f);
        }

        public PowerLoadSheddingProjection GetProjection()
        {
            var r = Evaluate();
            return new PowerLoadSheddingProjection
            {
                AvailableGenerationKw = r.AvailableGenerationKw,
                TotalDemandKw = r.TotalDemandKw,
                ServedLoadKw = r.ServedLoadKw,
                ShedLoadKw = r.ShedLoadKw,
                BrownoutRiskPermille = r.BrownoutRiskPermille,
                CascadingTripRiskPermille = r.CascadingTripRiskPermille,
                EnergyPovertyMoralePenaltyPermille = r.EnergyPovertyMoralePenaltyPermille,
                IsBlackout = r.IsBlackout,
                DemandCount = BuildDemands().Count,
                ShedConsumers = new List<string>(r.ShedConsumers),
                GridWearPermille = GetGridWearPermille()
            };
        }

        /// <summary>
        /// Morale points a full energy-poverty penalty removes from one survivor.
        /// Routed by the caller into the canonical morale owner
        /// (<c>NeedsSystem.Modify(id, NeedKind.Morale, delta)</c>).
        /// </summary>
        public float GetMoralePenaltyPoints(int penaltyPermille)
        {
            int clamped = Math.Max(0, Math.Min(PowerLoadSheddingEngine.PermilleScale, penaltyPermille));
            return -(MoralePointsAtFullPenalty * clamped) / PowerLoadSheddingEngine.PermilleScale;
        }
    }
}
