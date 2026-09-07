// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Combat
{
    // ── Catalog DTOs ─────────────────────────────────────────────────

    [Serializable]
    public sealed class BreachingObstacleDef
    {
        public string obstacle_id = string.Empty;
        public string display_name = string.Empty;
        public string category = string.Empty;
        public bool path_blocking = true;
        public float cover_rating;
        public float structural_rating = 1f;
        public List<string> clearance_tags = new List<string>();
        public List<string> hazard_tags = new List<string>();
        public bool preserves_partial_progress = true;
        public string notes = string.Empty;
    }

    [Serializable]
    public sealed class BreachingToolDef
    {
        public string tool_id = string.Empty;
        public string display_name = string.Empty;
        public string item_id = string.Empty;
        public List<string> supported_clearance_tags = new List<string>();
        public int setup_ticks = 1;
        public float clearance_power = 1f;
        public float noise;
        public float operator_exposure;
        public float durability_cost;
        public bool consumed_on_use;
        public bool requires_vehicle;
        public bool destroys_cover;
        /// <summary>Authored multiplier when vehicle support is available (requires_vehicle tools).</summary>
        public float vehicle_support_bonus = 1f;
        /// <summary>Fraction of cover remaining after a non-destroying clear (0 = wipe cover).</summary>
        public float cover_remnant_factor = 0.35f;
        public string notes = string.Empty;
    }

    /// <summary>Authored clearance balance table — no engine-local magic multipliers.</summary>
    [Serializable]
    public sealed class BreachingBalanceDef
    {
        public float skill_min = 0.1f;
        public float skill_max = 1.5f;
        public float condition_min = 0.15f;
        public float condition_max = 1.25f;
        public float default_vehicle_support_bonus = 1.25f;
        public float incident_skill_offset = 1.15f;
        public float incident_scale = 0.35f;
        public float incident_cap = 0.55f;
        public float damaged_stall_chance = 0.35f;
        public float default_cover_remnant_factor = 0.35f;
        public float setup_noise_scale = 0.25f;
        public float setup_wear_scale = 0.25f;
        public float hazard_secondary_chance = 0.2f;
    }

    [Serializable]
    public sealed class BreachingCatalog
    {
        public int schema_version = 1;
        public BreachingBalanceDef balance = new BreachingBalanceDef();
        public List<BreachingObstacleDef> obstacles = new List<BreachingObstacleDef>();
        public List<BreachingToolDef> tools = new List<BreachingToolDef>();
    }

    public sealed class BreachingEval
    {
        public bool CanBegin;
        public string FailureCode = string.Empty;
        public string ObstacleId = string.Empty;
        public string ToolId = string.Empty;
        public int SetupTicks;
        public int ClearTicks;
        public float EffectiveClearance;
        public float Noise;
        public float OperatorExposure;
        public bool DestroysCover;
        public bool ConsumesItem;
        public bool RequiresVehicle;
    }

    public sealed class BreachingTickResult
    {
        public bool Success;
        public string FailureCode = string.Empty;
        public string Phase = BreachPhaseIds.Available;
        public float Progress01;
        public float NoiseEmitted;
        public float WearApplied;
        public bool Cleared;
        public bool OperatorIncident;
        public bool PathOpen;
        public float CoverRemaining;
    }

    /// <summary>
    /// Plan B86 — tactical obstacle clearance authority.
    /// Validates tools against obstacle clearance tags, advances setup/clear
    /// ticks, emits noise/wear amounts, and mutates <see cref="BarrierState"/>.
    /// Does not own expedition inventory authority, route mine corridors, or
    /// railway track clearance. Explosive-style tools are fictionalized
    /// gameplay consumables only — no real construction fields.
    /// </summary>
    public sealed class CombatBreachingEngine
    {
        public const string CapabilityId = "obstacle_breaching";

        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private BreachingCatalog _catalog = new BreachingCatalog();
        private readonly Dictionary<string, BreachingObstacleDef> _obstacles =
            new Dictionary<string, BreachingObstacleDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, BreachingToolDef> _tools =
            new Dictionary<string, BreachingToolDef>(StringComparer.Ordinal);

        public CombatBreachingEngine(ISeededRng? rng = null, ILog? log = null)
        {
            _rng = rng ?? new SeededRng(86);
            _log = log ?? NullLog.Instance;
        }

        public IReadOnlyDictionary<string, BreachingObstacleDef> Obstacles => _obstacles;
        public IReadOnlyDictionary<string, BreachingToolDef> Tools => _tools;
        public BreachingBalanceDef Balance => _catalog.balance ?? new BreachingBalanceDef();

        public void LoadCatalog(BreachingCatalog? catalog)
        {
            _catalog = catalog ?? new BreachingCatalog();
            if (_catalog.balance == null)
                _catalog.balance = new BreachingBalanceDef();
            _obstacles.Clear();
            _tools.Clear();

            foreach (var o in _catalog.obstacles ?? new List<BreachingObstacleDef>())
            {
                if (o == null || string.IsNullOrWhiteSpace(o.obstacle_id)) continue;
                if (o.structural_rating <= 0f) continue;
                _obstacles[o.obstacle_id] = o;
            }

            foreach (var t in _catalog.tools ?? new List<BreachingToolDef>())
            {
                if (t == null || string.IsNullOrWhiteSpace(t.tool_id)) continue;
                if (t.clearance_power <= 0f) continue;
                _tools[t.tool_id] = t;
            }
        }

        public BreachingEval Evaluate(
            BarrierState barrier,
            string toolId,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f,
            bool vehicleAvailable = false,
            InventoryContainer? inventory = null)
        {
            var eval = new BreachingEval { ToolId = toolId ?? string.Empty };
            if (barrier == null)
            {
                eval.FailureCode = "missing_barrier";
                return eval;
            }

            if (string.Equals(barrier.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal))
            {
                eval.FailureCode = "already_cleared";
                return eval;
            }

            // Active clearance cannot be restarted; Interrupted must Advance, not Begin.
            if (string.Equals(barrier.BreachPhase, BreachPhaseIds.SettingUp, StringComparison.Ordinal)
                || string.Equals(barrier.BreachPhase, BreachPhaseIds.Clearing, StringComparison.Ordinal)
                || string.Equals(barrier.BreachPhase, BreachPhaseIds.Interrupted, StringComparison.Ordinal))
            {
                eval.FailureCode = "breach_in_progress";
                return eval;
            }

            string profileId = string.IsNullOrWhiteSpace(barrier.ObstacleProfileId)
                ? InferProfileFromMaterial(barrier.MaterialId)
                : barrier.ObstacleProfileId;
            if (!_obstacles.TryGetValue(profileId, out var obstacle))
            {
                eval.FailureCode = "unknown_obstacle";
                return eval;
            }

            if (!_tools.TryGetValue(toolId ?? string.Empty, out var tool))
            {
                eval.FailureCode = "unknown_tool";
                return eval;
            }

            // Abandoned with preserved progress resumes only with the same tool.
            bool resumingAbandoned =
                string.Equals(barrier.BreachPhase, BreachPhaseIds.Abandoned, StringComparison.Ordinal)
                && barrier.BreachProgress01 > 0f
                && !string.IsNullOrEmpty(barrier.ActiveBreachToolId);
            if (resumingAbandoned
                && !string.Equals(barrier.ActiveBreachToolId, tool.tool_id, StringComparison.Ordinal))
            {
                eval.FailureCode = "active_tool_mismatch";
                return eval;
            }

            if (!TagsOverlap(obstacle.clearance_tags, tool.supported_clearance_tags))
            {
                eval.FailureCode = "unsupported_method";
                return eval;
            }

            if (tool.requires_vehicle && !vehicleAvailable)
            {
                eval.FailureCode = "vehicle_required";
                return eval;
            }

            // Fail closed: tools with an item_id require bound inventory logistics.
            if (!string.IsNullOrEmpty(tool.item_id) && inventory == null)
            {
                eval.FailureCode = "logistics_unbound";
                return eval;
            }

            if (!string.IsNullOrEmpty(tool.item_id) && inventory!.CountById(tool.item_id) <= 0)
            {
                // Resume with preserved progress does not re-require a fresh item
                // when the prior Begin already reserved/consumed it.
                if (!(resumingAbandoned && tool.consumed_on_use))
                {
                    eval.FailureCode = "missing_tool_item";
                    return eval;
                }
            }

            var bal = Balance;
            float skill = Math.Clamp(operatorSkill01, bal.skill_min, bal.skill_max);
            float condition = Math.Clamp(equipmentCondition01, bal.condition_min, bal.condition_max);
            float vehicleMod = 1f;
            if (tool.requires_vehicle && vehicleAvailable)
            {
                float bonus = tool.vehicle_support_bonus > 0f
                    ? tool.vehicle_support_bonus
                    : bal.default_vehicle_support_bonus;
                vehicleMod = Math.Max(1f, bonus);
            }

            float effective = Math.Max(0.5f,
                tool.clearance_power * skill * condition * vehicleMod);
            int clearTicks = (int)Math.Ceiling(obstacle.structural_rating / effective);
            clearTicks = Math.Clamp(clearTicks, 1, 30);

            eval.CanBegin = true;
            eval.ObstacleId = obstacle.obstacle_id;
            eval.SetupTicks = Math.Max(0, tool.setup_ticks);
            eval.ClearTicks = clearTicks;
            eval.EffectiveClearance = effective;
            eval.Noise = Math.Clamp(tool.noise, 0f, 1f);
            eval.OperatorExposure = Math.Clamp(tool.operator_exposure, 0f, 1f);
            eval.DestroysCover = tool.destroys_cover;
            eval.ConsumesItem = tool.consumed_on_use && !resumingAbandoned;
            eval.RequiresVehicle = tool.requires_vehicle;
            return eval;
        }

        public BreachingTickResult Begin(
            BarrierState barrier,
            string toolId,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f,
            bool vehicleAvailable = false,
            InventoryContainer? inventory = null)
        {
            var result = new BreachingTickResult();
            var eval = Evaluate(barrier, toolId, operatorSkill01, equipmentCondition01, vehicleAvailable, inventory);
            if (!eval.CanBegin)
            {
                result.FailureCode = eval.FailureCode;
                result.Phase = barrier?.BreachPhase ?? BreachPhaseIds.Available;
                return result;
            }

            if (!_tools.TryGetValue(toolId, out var tool) || !_obstacles.TryGetValue(eval.ObstacleId, out var obstacle))
            {
                result.FailureCode = "catalog_miss";
                return result;
            }

            bool resumingAbandoned =
                string.Equals(barrier.BreachPhase, BreachPhaseIds.Abandoned, StringComparison.Ordinal)
                && barrier.BreachProgress01 > 0f
                && string.Equals(barrier.ActiveBreachToolId, tool.tool_id, StringComparison.Ordinal);

            if (eval.ConsumesItem && inventory != null)
            {
                var bill = new InventoryBill().AddCost(tool.item_id, 1);
                bool ok = inventory.TryExecuteTransaction(bill, () => { });
                if (!ok)
                {
                    result.FailureCode = "consume_failed";
                    return result;
                }
            }

            var bal = Balance;
            barrier.ObstacleProfileId = obstacle.obstacle_id;
            barrier.ActiveBreachToolId = tool.tool_id;
            barrier.BreachDestroysCover = tool.destroys_cover;
            barrier.CoverContribution = Math.Clamp(obstacle.cover_rating, 0f, 1f);
            barrier.PathBlocking = obstacle.path_blocking ? 1f : 0f;

            if (resumingAbandoned)
            {
                // Keep remaining ticks / progress; re-enter Clearing (setup already done).
                if (barrier.BreachClearTicksTotal <= 0)
                    barrier.BreachClearTicksTotal = eval.ClearTicks;
                if (barrier.BreachClearTicksRemaining <= 0 && barrier.BreachProgress01 < 1f)
                    barrier.BreachClearTicksRemaining = eval.ClearTicks;
                barrier.BreachPhase = BreachPhaseIds.Clearing;
                barrier.BreachSetupTicksRemaining = 0;
            }
            else
            {
                barrier.BreachClearTicksTotal = eval.ClearTicks;
                barrier.BreachClearTicksRemaining = eval.ClearTicks;
                barrier.BreachProgress01 = 0f;

                if (eval.SetupTicks > 0)
                {
                    barrier.BreachPhase = BreachPhaseIds.SettingUp;
                    barrier.BreachSetupTicksRemaining = eval.SetupTicks;
                }
                else
                {
                    barrier.BreachPhase = BreachPhaseIds.Clearing;
                    barrier.BreachSetupTicksRemaining = 0;
                }
            }

            result.Success = true;
            result.Phase = barrier.BreachPhase;
            result.Progress01 = barrier.BreachProgress01;
            result.NoiseEmitted = eval.Noise * Math.Clamp(bal.setup_noise_scale, 0f, 1f);
            result.WearApplied = tool.consumed_on_use
                ? 0f
                : tool.durability_cost * Math.Clamp(bal.setup_wear_scale, 0f, 1f);
            result.CoverRemaining = barrier.CoverContribution;
            result.PathOpen = barrier.PathBlocking <= 0.01f;
            return result;
        }

        public BreachingTickResult Advance(
            BarrierState barrier,
            float operatorSkill01 = 0.5f,
            float equipmentCondition01 = 1f,
            ISeededRng? rng = null)
        {
            var result = new BreachingTickResult();
            var roll = rng ?? _rng;
            if (barrier == null)
            {
                result.FailureCode = "missing_barrier";
                return result;
            }

            if (!_tools.TryGetValue(barrier.ActiveBreachToolId ?? string.Empty, out var tool)
                || !_obstacles.TryGetValue(barrier.ObstacleProfileId ?? string.Empty, out var obstacle))
            {
                result.FailureCode = "no_active_breach";
                result.Phase = barrier.BreachPhase ?? BreachPhaseIds.Available;
                return result;
            }

            if (string.Equals(barrier.BreachPhase, BreachPhaseIds.Cleared, StringComparison.Ordinal))
            {
                result.Success = true;
                result.Cleared = true;
                result.PathOpen = true;
                result.Phase = BreachPhaseIds.Cleared;
                result.Progress01 = 1f;
                return result;
            }

            if (string.Equals(barrier.BreachPhase, BreachPhaseIds.SettingUp, StringComparison.Ordinal))
            {
                barrier.BreachSetupTicksRemaining = Math.Max(0, barrier.BreachSetupTicksRemaining - 1);
                result.NoiseEmitted = Math.Clamp(tool.noise * 0.2f, 0f, 1f);
                result.WearApplied = tool.consumed_on_use ? 0f : tool.durability_cost * 0.1f;
                if (barrier.BreachSetupTicksRemaining <= 0)
                    barrier.BreachPhase = BreachPhaseIds.Clearing;
                result.Success = true;
                result.Phase = barrier.BreachPhase;
                result.Progress01 = barrier.BreachProgress01;
                result.CoverRemaining = barrier.CoverContribution;
                return result;
            }

            bool resumablePhase =
                string.Equals(barrier.BreachPhase, BreachPhaseIds.Clearing, StringComparison.Ordinal)
                || string.Equals(barrier.BreachPhase, BreachPhaseIds.Interrupted, StringComparison.Ordinal)
                || string.Equals(barrier.BreachPhase, BreachPhaseIds.Abandoned, StringComparison.Ordinal);
            if (!resumablePhase)
            {
                result.FailureCode = "invalid_phase";
                result.Phase = barrier.BreachPhase;
                return result;
            }

            if (string.Equals(barrier.BreachPhase, BreachPhaseIds.Interrupted, StringComparison.Ordinal)
                || string.Equals(barrier.BreachPhase, BreachPhaseIds.Abandoned, StringComparison.Ordinal))
            {
                if (!obstacle.preserves_partial_progress)
                {
                    result.FailureCode = "progress_lost";
                    result.Phase = BreachPhaseIds.Failed;
                    barrier.BreachPhase = BreachPhaseIds.Failed;
                    return result;
                }
                barrier.BreachPhase = BreachPhaseIds.Clearing;
            }

            var bal = Balance;
            float skill = Math.Clamp(operatorSkill01, bal.skill_min, bal.skill_max);
            float condition = Math.Clamp(equipmentCondition01, bal.condition_min, bal.condition_max);
            float incidentRisk = Math.Clamp(
                tool.operator_exposure * (bal.incident_skill_offset - skill) * bal.incident_scale,
                0f,
                bal.incident_cap);
            if (roll.NextDouble() < incidentRisk)
            {
                result.OperatorIncident = true;
                result.NoiseEmitted = Math.Clamp(tool.noise + 0.15f, 0f, 1f);
                result.WearApplied = tool.durability_cost;
                if (!obstacle.preserves_partial_progress)
                {
                    barrier.BreachPhase = BreachPhaseIds.Failed;
                    result.FailureCode = "operator_incident";
                    result.Phase = BreachPhaseIds.Failed;
                    result.Success = false;
                    return result;
                }
                barrier.BreachPhase = BreachPhaseIds.Interrupted;
                result.Success = true;
                result.Phase = BreachPhaseIds.Interrupted;
                result.Progress01 = barrier.BreachProgress01;
                result.CoverRemaining = barrier.CoverContribution;
                return result;
            }

            barrier.BreachClearTicksRemaining = Math.Max(0, barrier.BreachClearTicksRemaining - 1);
            int total = Math.Max(1, barrier.BreachClearTicksTotal);
            barrier.BreachProgress01 = Math.Clamp(1f - (barrier.BreachClearTicksRemaining / (float)total), 0f, 1f);
            if (condition < 0.5f
                && barrier.BreachClearTicksRemaining > 0
                && roll.NextDouble() < Math.Clamp(bal.damaged_stall_chance, 0f, 1f))
            {
                barrier.BreachClearTicksRemaining += 1;
            }

            result.NoiseEmitted = Math.Clamp(tool.noise, 0f, 1f);
            result.WearApplied = tool.consumed_on_use ? 0f : tool.durability_cost;
            result.Success = true;
            result.Progress01 = barrier.BreachProgress01;
            result.CoverRemaining = barrier.CoverContribution;

            if (barrier.BreachClearTicksRemaining <= 0)
            {
                barrier.BreachPhase = BreachPhaseIds.Cleared;
                barrier.PathBlocking = 0f;
                barrier.IntegrityPct = 0f;
                if (tool.destroys_cover)
                {
                    barrier.CoverContribution = 0f;
                }
                else
                {
                    float remnant = tool.cover_remnant_factor >= 0f
                        ? tool.cover_remnant_factor
                        : bal.default_cover_remnant_factor;
                    barrier.CoverContribution = Math.Max(0f, barrier.CoverContribution * Math.Clamp(remnant, 0f, 1f));
                }
                result.Cleared = true;
                result.PathOpen = true;
                result.Phase = BreachPhaseIds.Cleared;
                result.Progress01 = 1f;
                result.CoverRemaining = barrier.CoverContribution;

                if (obstacle.hazard_tags != null && obstacle.hazard_tags.Count > 0
                    && roll.NextDouble() < Math.Clamp(bal.hazard_secondary_chance, 0f, 1f))
                {
                    result.NoiseEmitted = Math.Clamp(result.NoiseEmitted + 0.1f, 0f, 1f);
                }
            }
            else
            {
                barrier.BreachPhase = BreachPhaseIds.Clearing;
                result.Phase = BreachPhaseIds.Clearing;
            }

            return result;
        }

        public BreachingTickResult Abandon(BarrierState barrier)
        {
            var result = new BreachingTickResult();
            if (barrier == null)
            {
                result.FailureCode = "missing_barrier";
                return result;
            }

            if (!_obstacles.TryGetValue(barrier.ObstacleProfileId ?? string.Empty, out var obstacle))
            {
                barrier.BreachPhase = BreachPhaseIds.Abandoned;
                barrier.ActiveBreachToolId = string.Empty;
                barrier.BreachProgress01 = 0f;
                result.Success = true;
                result.Phase = BreachPhaseIds.Abandoned;
                return result;
            }

            if (!obstacle.preserves_partial_progress)
            {
                barrier.BreachProgress01 = 0f;
                barrier.BreachClearTicksRemaining = barrier.BreachClearTicksTotal;
                barrier.ActiveBreachToolId = string.Empty;
            }
            // When progress is preserved, keep ActiveBreachToolId so Begin/Advance can resume.

            barrier.BreachPhase = BreachPhaseIds.Abandoned;
            barrier.BreachSetupTicksRemaining = 0;
            result.Success = true;
            result.Phase = BreachPhaseIds.Abandoned;
            result.Progress01 = barrier.BreachProgress01;
            result.CoverRemaining = barrier.CoverContribution;
            result.PathOpen = barrier.PathBlocking <= 0.01f;
            return result;
        }

        public static string InferProfileFromMaterial(string materialId)
        {
            if (string.IsNullOrWhiteSpace(materialId)) return "obstacle_debris_choke";
            if (materialId.IndexOf("rebar", StringComparison.OrdinalIgnoreCase) >= 0)
                return "obstacle_reinforced_fence";
            if (materialId.IndexOf("concrete", StringComparison.OrdinalIgnoreCase) >= 0)
                return "obstacle_sandbag_redoubt";
            if (materialId.IndexOf("metal", StringComparison.OrdinalIgnoreCase) >= 0)
                return "obstacle_barricaded_gate";
            if (materialId.IndexOf("wood", StringComparison.OrdinalIgnoreCase) >= 0)
                return "obstacle_debris_choke";
            return "obstacle_debris_choke";
        }

        private static bool TagsOverlap(List<string>? a, List<string>? b)
        {
            if (a == null || b == null || a.Count == 0 || b.Count == 0) return false;
            for (int i = 0; i < a.Count; i++)
            {
                string left = a[i] ?? string.Empty;
                if (string.IsNullOrWhiteSpace(left)) continue;
                for (int j = 0; j < b.Count; j++)
                {
                    if (string.Equals(left, b[j], StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }
    }

    public static class BreachingCatalogLoader
    {
        public const string DefaultFileName = "breaching_equipment_catalog.json";

        public static BreachingCatalog Load(
            string dataDirectory,
            IFileIO files,
            IJsonSerializer json,
            ILog? log = null)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));
            string path = Path.Combine(dataDirectory ?? string.Empty, DefaultFileName);
            if (!files.FileExists(path))
            {
                log?.Warn($"[Breaching] catalog missing at {path}; idle empty catalog.");
                return new BreachingCatalog();
            }

            string text = files.ReadAllText(path);
            return json.Deserialize<BreachingCatalog>(text)
                   ?? throw new InvalidOperationException("Failed to deserialize breaching_equipment_catalog.json");
        }

        public static void Validate(BreachingCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));
            if (catalog.schema_version < 1)
                throw new InvalidOperationException("breaching_equipment_catalog schema_version must be >= 1");
            if (catalog.obstacles == null || catalog.obstacles.Count == 0)
                throw new InvalidOperationException("breaching_equipment_catalog requires obstacles");
            if (catalog.tools == null || catalog.tools.Count == 0)
                throw new InvalidOperationException("breaching_equipment_catalog requires tools");

            RejectDupes(catalog.obstacles, o => o.obstacle_id, "obstacle_id");
            RejectDupes(catalog.tools, t => t.tool_id, "tool_id");

            foreach (var o in catalog.obstacles)
            {
                if (o.structural_rating <= 0f)
                    throw new InvalidOperationException($"obstacle {o.obstacle_id} structural_rating must be > 0");
            }

            foreach (var t in catalog.tools)
            {
                if (t.clearance_power <= 0f)
                    throw new InvalidOperationException($"tool {t.tool_id} clearance_power must be > 0");
                if (string.IsNullOrWhiteSpace(t.item_id))
                    throw new InvalidOperationException($"tool {t.tool_id} requires item_id");
            }
        }

        private static void RejectDupes<T>(List<T>? items, Func<T, string> idOf, string label)
        {
            if (items == null) return;
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in items)
            {
                if (item == null) continue;
                string id = idOf(item) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id)) continue;
                if (!seen.Add(id))
                    throw new InvalidOperationException($"Duplicate breaching {label}: {id}");
            }
        }
    }
}
