// SPDX-License-Identifier: MIT
using System;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// Plan B86 — tactical obstacle clearance actions on the existing barrier authority.
    /// Progress lives in <see cref="CombatState.Barriers"/> (combat save section).
    /// Noise/wear are projected through optional <see cref="CombatHostPorts"/> sinks.
    /// </summary>
    public partial class TacticalCombatSystem
    {
        private CombatBreachingEngine? _breaching;
        private InventoryContainer? _breachInventory;
        private bool _breachVehicleAvailable;
        private float _breachEncounterNoise;

        /// <summary>Cumulative breach noise emitted this encounter (0..∞, typically &lt; 5).</summary>
        public float BreachEncounterNoise => _breachEncounterNoise;

        public CombatBreachingEngine EnsureBreachingEngine(ISeededRng? rng = null)
        {
            if (_breaching == null)
            {
                int seed = _state != null && _state.Seed != 0
                    ? unchecked(_state.Seed ^ 86)
                    : 86;
                _breaching = new CombatBreachingEngine(rng ?? new SeededRng(seed));
            }
            return _breaching;
        }

        public void LoadBreachingCatalog(BreachingCatalog? catalog)
        {
            EnsureBreachingEngine().LoadCatalog(catalog);
        }

        /// <summary>
        /// Bind expedition logistics for consumable/tool checks without taking
        /// ownership of inventory or vehicle garage state.
        /// </summary>
        public void ConfigureBreachingLogistics(InventoryContainer? inventory, bool vehicleAvailable)
        {
            _breachInventory = inventory;
            _breachVehicleAvailable = vehicleAvailable;
        }

        /// <summary>
        /// Ensure a barrier row exists for the given obstacle profile (encounter-local).
        /// Reuses an existing id when present; otherwise appends a new barrier.
        /// </summary>
        public BarrierState EnsureObstacleBarrier(
            string barrierId,
            string obstacleProfileId,
            int lane = (int)CombatLane.Center,
            bool isPlayer = true,
            string materialId = "")
        {
            if (string.IsNullOrWhiteSpace(barrierId))
                throw new ArgumentException("barrierId required", nameof(barrierId));

            var existing = FindBarrier(barrierId);
            if (existing != null)
            {
                if (!string.IsNullOrWhiteSpace(obstacleProfileId))
                    existing.ObstacleProfileId = obstacleProfileId;
                return existing;
            }

            var engine = EnsureBreachingEngine();
            BreachingObstacleDef? profile = null;
            if (!string.IsNullOrWhiteSpace(obstacleProfileId))
                engine.Obstacles.TryGetValue(obstacleProfileId, out profile);

            var barrier = new BarrierState
            {
                Id = barrierId,
                Lane = Math.Clamp(lane, 0, 2),
                IsPlayer = isPlayer,
                MaterialId = materialId ?? string.Empty,
                IntegrityPct = 100f,
                ObstacleProfileId = profile?.obstacle_id
                    ?? (string.IsNullOrWhiteSpace(obstacleProfileId)
                        ? CombatBreachingEngine.InferProfileFromMaterial(materialId)
                        : obstacleProfileId),
                BreachPhase = BreachPhaseIds.Available,
                PathBlocking = profile == null || profile.path_blocking ? 1f : 0f,
                CoverContribution = profile != null ? Math.Clamp(profile.cover_rating, 0f, 1f) : 0.3f
            };
            _state.Barriers.Add(barrier);
            Notify();
            return barrier;
        }

        public BarrierState? FindBarrier(string barrierId)
        {
            if (string.IsNullOrEmpty(barrierId) || _state?.Barriers == null) return null;
            for (int i = 0; i < _state.Barriers.Count; i++)
            {
                var b = _state.Barriers[i];
                if (b != null && string.Equals(b.Id, barrierId, StringComparison.Ordinal))
                    return b;
            }
            return null;
        }

        public ActionPreflight EvaluateBreach(
            string barrierId,
            string toolId,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f)
        {
            if (_state.Resolved) return ActionPreflight.Blocked("Encounter is resolved");
            if (_state.Phase != (int)CombatPhase.PlayerTurn) return ActionPreflight.Blocked("Not player turn");
            var barrier = FindBarrier(barrierId);
            if (barrier == null) return ActionPreflight.Blocked("Barrier not found");
            var eval = EnsureBreachingEngine().Evaluate(
                barrier, toolId, operatorSkill01, equipmentCondition01,
                _breachVehicleAvailable, _breachInventory);
            if (!eval.CanBegin)
                return ActionPreflight.Blocked(eval.FailureCode);
            return ActionPreflight.Ok;
        }

        public CombatActionResult BeginBreach(
            string barrierId,
            string toolId,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            if (_state.Phase != (int)CombatPhase.PlayerTurn)
            {
                res.Message = "Not player turn.";
                return res;
            }

            var barrier = FindBarrier(barrierId);
            if (barrier == null)
            {
                res.Message = "Barrier not found: " + barrierId;
                return res;
            }

            var tick = EnsureBreachingEngine().Begin(
                barrier, toolId, operatorSkill01, equipmentCondition01,
                _breachVehicleAvailable, _breachInventory);
            if (!tick.Success)
            {
                res.Message = "Breach refused: " + tick.FailureCode;
                return res;
            }

            ApplyBreachSideEffects(tick, toolId);
            AddEvent("breach_begin", barrierId,
                $"Breach begun with {toolId} → {tick.Phase} (noise={tick.NoiseEmitted:F2})");
            res.Success = true;
            res.Message = tick.Phase;
            Notify();
            return res;
        }

        public CombatActionResult AdvanceBreach(
            string barrierId,
            ISeededRng rng,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            if (_state.Phase != (int)CombatPhase.PlayerTurn)
            {
                res.Message = "Not player turn.";
                return res;
            }

            var barrier = FindBarrier(barrierId);
            if (barrier == null)
            {
                res.Message = "Barrier not found: " + barrierId;
                return res;
            }

            var tick = EnsureBreachingEngine().Advance(
                barrier, operatorSkill01, equipmentCondition01, rng);
            if (!tick.Success && !string.IsNullOrEmpty(tick.FailureCode))
            {
                ApplyBreachSideEffects(tick, barrier.ActiveBreachToolId);
                AddEvent("breach_fail", barrierId, "Breach failed: " + tick.FailureCode);
                res.Message = tick.FailureCode;
                Notify();
                return res;
            }

            ApplyBreachSideEffects(tick, barrier.ActiveBreachToolId);
            string detail = tick.Cleared
                ? $"Obstacle cleared (path open, cover={tick.CoverRemaining:F2})"
                : $"Breach {tick.Phase} progress={tick.Progress01:F2}";
            if (tick.OperatorIncident)
                detail += " [operator incident]";
            AddEvent(tick.Cleared ? "breach_cleared" : "breach_advance", barrierId, detail);
            res.Success = true;
            res.Message = detail;
            Notify();
            return res;
        }

        public CombatActionResult AbandonBreach(string barrierId)
        {
            var res = new CombatActionResult();
            if (_state.Resolved) { res.Message = "Encounter is over."; return res; }
            var barrier = FindBarrier(barrierId);
            if (barrier == null)
            {
                res.Message = "Barrier not found: " + barrierId;
                return res;
            }

            var tick = EnsureBreachingEngine().Abandon(barrier);
            AddEvent("breach_abandon", barrierId,
                $"Breach abandoned (progress={tick.Progress01:F2})");
            res.Success = tick.Success;
            res.Message = tick.Phase;
            Notify();
            return res;
        }

        private void ApplyBreachSideEffects(BreachingTickResult tick, string? toolId)
        {
            if (tick == null) return;
            if (tick.NoiseEmitted > 0f)
            {
                _breachEncounterNoise += tick.NoiseEmitted;
                if (_ports?.EmitBreachNoise != null)
                    _ports.EmitBreachNoise.Invoke(tick.NoiseEmitted);
                else
                    AddEvent("breach_noise", toolId ?? string.Empty, $"noise={tick.NoiseEmitted:F2}");
            }

            if (tick.WearApplied > 0f && !string.IsNullOrEmpty(toolId)
                && _breaching != null
                && _breaching.Tools.TryGetValue(toolId, out var tool)
                && !string.IsNullOrEmpty(tool.item_id))
            {
                if (_ports?.ApplyBreachToolWear != null)
                    _ports.ApplyBreachToolWear.Invoke(tool.item_id, tick.WearApplied);
                else
                    AddEvent("breach_wear", tool.item_id, $"wear={tick.WearApplied:F2}");
            }
        }
    }
}
