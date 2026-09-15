// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core.Foundry
{
    /// <summary>Authored extrusion product profile (Plan 140 Phase 1). Abstract industrial data.</summary>
    public sealed class ExtrusionProductDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string billet_material_tag { get; set; } = string.Empty;
        public string machine_class { get; set; } = string.Empty;
        public string die_profile_id { get; set; } = string.Empty;
        public int energy_cost { get; set; }
        public int cooling_requirement { get; set; }
        public int tool_wear { get; set; }
        public int base_quality { get; set; } = 55;
        public int defect_risk_bp { get; set; }
        public List<string> operator_skill_tags { get; set; } = new List<string>();
        public string result_item_id { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    public sealed class ExtrusionMachineDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int alignment_quality { get; set; } = 70;
        public int power_stability_required_bp { get; set; } = 40;
        public int max_defect_reduction_bp { get; set; } = 400;
    }

    public sealed class ExtrusionBilletDef
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int quality_bp { get; set; } = 60;
        public string rarity { get; set; } = "common";
    }

    public sealed class HydraulicExtrusionCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<ExtrusionProductDef> product_profiles { get; set; } = new List<ExtrusionProductDef>();
        public List<ExtrusionMachineDef> machine_profiles { get; set; } = new List<ExtrusionMachineDef>();
        public List<ExtrusionBilletDef> billet_materials { get; set; } = new List<ExtrusionBilletDef>();

        private readonly Dictionary<string, ExtrusionProductDef> _products = new Dictionary<string, ExtrusionProductDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, ExtrusionMachineDef> _machines = new Dictionary<string, ExtrusionMachineDef>(StringComparer.Ordinal);
        private readonly Dictionary<string, ExtrusionBilletDef> _billets = new Dictionary<string, ExtrusionBilletDef>(StringComparer.Ordinal);

        public void Index()
        {
            _products.Clear();
            _machines.Clear();
            _billets.Clear();
            foreach (var p in product_profiles) if (p != null && !string.IsNullOrEmpty(p.id)) _products[p.id] = p;
            foreach (var m in machine_profiles) if (m != null && !string.IsNullOrEmpty(m.id)) _machines[m.id] = m;
            foreach (var b in billet_materials) if (b != null && !string.IsNullOrEmpty(b.id)) _billets[b.id] = b;
        }

        public ExtrusionProductDef? GetProduct(string id) { _products.TryGetValue(id ?? string.Empty, out var d); return d; }
        public ExtrusionMachineDef? GetMachine(string id) { _machines.TryGetValue(id ?? string.Empty, out var d); return d; }
        public ExtrusionBilletDef? GetBillet(string id) { _billets.TryGetValue(id ?? string.Empty, out var d); return d; }

        public IReadOnlyCollection<ExtrusionProductDef> AllProducts => _products.Values;
        public IReadOnlyCollection<ExtrusionMachineDef> AllMachines => _machines.Values;
    }

    public static class HydraulicExtrusionCatalogLoader
    {
        public const string DefaultFileName = "hydraulic_extrusion_catalog.json";

        public static HydraulicExtrusionCatalog Load(string dataDir, IFileIO fileIo)
        {
            if (fileIo == null || string.IsNullOrEmpty(dataDir)) return Empty();
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIo.FileExists(path)) return Empty();
            string json = fileIo.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return Empty();
            var catalog = JsonSerializer.Deserialize<HydraulicExtrusionCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? Empty();
            catalog.Index();
            return catalog;
        }

        private static HydraulicExtrusionCatalog Empty() { var c = new HydraulicExtrusionCatalog(); c.Index(); return c; }
    }

    /// <summary>Output quality classes. Never "perfect"; every class remains fallible downstream.</summary>
    public static class ExtrusionQuality
    {
        public const string Rejected = "rejected";
        public const string Utility = "utility";
        public const string HighPressure = "high_pressure";
        public const string Premium = "premium";
    }

    [Serializable]
    public sealed class ExtrusionMachineState
    {
        public string MachineId { get; set; } = string.Empty;
        public int ToolingConditionBp { get; set; } = 100;
        public int DieConditionBp { get; set; } = 100;
        public int OperatingHours { get; set; }
        public string ActiveBatchId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ExtrusionBatch
    {
        public string BatchId { get; set; } = string.Empty;
        public string ProductProfileId { get; set; } = string.Empty;
        public string MachineId { get; set; } = string.Empty;
        public int Units { get; set; }
        public int PhaseIndex { get; set; }
        public string Phase { get; set; } = HydraulicExtrusionEngine.Phases[0];
        public string QualityClass { get; set; } = string.Empty;
        public string DefectCode { get; set; } = string.Empty;
        public int BilletQualityBp { get; set; } = 60;
        public int FinalQualityScore { get; set; }
        public int Day { get; set; }
        public bool Completed { get; set; }
    }

    [Serializable]
    public sealed class HydraulicExtrusionState
    {
        public string SystemId { get; set; } = HydraulicExtrusionEngine.SystemId;
        public int SchemaVersion { get; set; } = 1;
        public List<ExtrusionMachineState> Machines { get; set; } = new List<ExtrusionMachineState>();
        public List<ExtrusionBatch> Batches { get; set; } = new List<ExtrusionBatch>();
        public int NextBatchSeq { get; set; }
        public int TotalCompleted { get; set; }
    }

    /// <summary>
    /// Plan 140 Phase 1 — advanced hydraulic extrusion. Produces high-integrity
    /// seamless tubing through canonical material/energy/cooling inputs. Quality
    /// and defects are deterministic; downstream reliability improves but never
    /// becomes absolute. Inventory owns produced stock after completion.
    /// </summary>
    public sealed class HydraulicExtrusionEngine
    {
        public const string SystemId = "hydraulic_extrusion";

        public static readonly string[] Phases =
        {
            "billet_conditioning", "forming", "piercing", "sizing", "finishing", "qa"
        };

        public const int MinQualityScore = 0;
        public const int MaxQualityScore = 100;
        /// <summary>Maximum downstream failure-risk reduction any grade may grant (basis points).</summary>
        public const int MaxReliabilityBenefitBp = 1500;

        private HydraulicExtrusionState _state = new HydraulicExtrusionState();
        private readonly HydraulicExtrusionCatalog _catalog;
        private readonly ILog _log;

        public ISeededRng? Rng { get; set; }

        public HydraulicExtrusionState State => _state;
        public HydraulicExtrusionCatalog Catalog => _catalog;

        public HydraulicExtrusionEngine(HydraulicExtrusionCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult RegisterMachine(string machineId)
        {
            var def = _catalog.GetMachine(machineId);
            if (def == null)
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_unavailable");
            if (FindMachine(machineId) != null)
                return ActionResult.Success("extrusion.machine_present");

            _state.Machines.Add(new ExtrusionMachineState { MachineId = machineId });
            return ActionResult.Success("extrusion.machine_registered");
        }

        public ExtrusionMachineState? FindMachine(string machineId)
            => _state.Machines.Find(m => string.Equals(m.MachineId, machineId, StringComparison.Ordinal));

        /// <summary>
        /// Starts a batch. Energy and cooling are availability reads (0..100) supplied
        /// by the canonical power/water authorities; below requirement the batch is refused.
        /// </summary>
        public ActionResult StartBatch(
            string productProfileId,
            string machineId,
            int units,
            int billetQualityBp,
            int energyAvailableBp,
            int coolingAvailableBp,
            int day)
        {
            var product = _catalog.GetProduct(productProfileId);
            var machineDef = _catalog.GetMachine(machineId);
            if (product == null)
                return ActionResult.Blocked("material_incompatible", "extrusion.material_incompatible");
            if (machineDef == null)
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_unavailable");
            if (units <= 0)
                return ActionResult.Blocked("material_incompatible", "extrusion.invalid_units");
            if (!string.Equals(product.machine_class, machineId, StringComparison.Ordinal))
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_class_mismatch");
            if (energyAvailableBp < product.energy_cost)
                return ActionResult.Blocked("power_unavailable", "extrusion.power_unavailable");
            if (coolingAvailableBp < product.cooling_requirement)
                return ActionResult.Blocked("cooling_unavailable", "extrusion.cooling_unavailable");

            var machine = FindMachine(machineId);
            if (machine == null)
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_unavailable");
            if (!string.IsNullOrEmpty(machine.ActiveBatchId))
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_busy");

            _state.NextBatchSeq++;
            var batch = new ExtrusionBatch
            {
                BatchId = $"extrusion_{day}_{_state.NextBatchSeq}",
                ProductProfileId = productProfileId,
                MachineId = machineId,
                Units = units,
                PhaseIndex = 0,
                Phase = Phases[0],
                BilletQualityBp = ClampBp(billetQualityBp),
                Day = day
            };
            _state.Batches.Add(batch);
            machine.ActiveBatchId = batch.BatchId;
            machine.OperatingHours += units;
            return ActionResult.Success("extrusion.batch_started");
        }

        /// <summary>Advances one abstract production phase. Refuses an already-completed batch.</summary>
        public ActionResult AdvanceBatch(string batchId)
        {
            var batch = FindBatch(batchId);
            if (batch == null)
                return ActionResult.Blocked("quality_control_failed", "extrusion.unknown_batch");
            if (batch.Completed)
                return ActionResult.Blocked("quality_control_failed", "extrusion.batch_complete");
            if (batch.PhaseIndex >= Phases.Length - 1)
                return ActionResult.Success("extrusion.batch_ready");

            batch.PhaseIndex++;
            batch.Phase = Phases[batch.PhaseIndex];
            return ActionResult.Success("extrusion.batch_advanced");
        }

        /// <summary>
        /// Completes a batch at QA, applying deterministic defect risk and tool wear.
        /// Material is conserved; rejected output is scrap, never silently perfect.
        /// </summary>
        public ActionResult CompleteBatch(string batchId, double operatorSkill)
        {
            var batch = FindBatch(batchId);
            if (batch == null)
                return ActionResult.Blocked("quality_control_failed", "extrusion.unknown_batch");
            if (batch.Completed)
                return ActionResult.Blocked("quality_control_failed", "extrusion.batch_complete");
            if (batch.PhaseIndex < Phases.Length - 1)
                return ActionResult.Blocked("quality_control_failed", "extrusion.batch_not_ready");

            var product = _catalog.GetProduct(batch.ProductProfileId);
            var machineDef = _catalog.GetMachine(batch.MachineId);
            var machine = FindMachine(batch.MachineId);
            if (product == null || machineDef == null || machine == null)
                return ActionResult.Blocked("machine_unavailable", "extrusion.machine_unavailable");

            int tooling = machine.ToolingConditionBp;
            double score = product.base_quality
                + (batch.BilletQualityBp - 50) / 50.0 * 15.0
                + (tooling - 50) / 50.0 * 10.0
                + Clamp01(operatorSkill) * 10.0
                + (machineDef.alignment_quality - 70) / 30.0 * 5.0;

            int defectRisk = Math.Max(50, product.defect_risk_bp
                - machineDef.max_defect_reduction_bp
                + (50 - tooling) * 20);
            bool defect = false;
            if (Rng != null)
                defect = Rng.Next(0, 10000) < defectRisk;
            else
                defect = StableDefect(batch.BatchId, defectRisk);

            if (defect)
            {
                score -= 18.0;
                batch.DefectCode = "dimensional_out_of_spec";
            }

            int finalScore = Math.Max(MinQualityScore, Math.Min(MaxQualityScore, (int)Math.Round(score)));
            batch.FinalQualityScore = finalScore;
            batch.QualityClass = Classify(finalScore);
            batch.Completed = true;

            machine.ToolingConditionBp = Math.Max(0, machine.ToolingConditionBp - product.tool_wear);
            machine.DieConditionBp = Math.Max(0, machine.DieConditionBp - Math.Max(1, product.tool_wear / 4));
            machine.ActiveBatchId = string.Empty;
            _state.TotalCompleted++;

            _log.Info($"[Extrusion] {batch.BatchId} complete: {batch.QualityClass} ({finalScore}), defect={defect}");
            return ActionResult.Success("extrusion.batch_completed");
        }

        /// <summary>
        /// Bounded downstream reliability benefit from a quality class. Never
        /// removes failure risk entirely; rejected stock grants nothing.
        /// </summary>
        public static int ReliabilityBenefitBp(string qualityClass) => qualityClass switch
        {
            ExtrusionQuality.Premium => 1000,
            ExtrusionQuality.HighPressure => 600,
            ExtrusionQuality.Utility => 250,
            _ => 0
        };

        /// <summary>Generic compatibility tag downstream utilities may consume.</summary>
        public string QualityTagFor(string batchId)
        {
            var batch = FindBatch(batchId);
            return batch == null || !batch.Completed
                ? "tubing_quality:none"
                : $"tubing_quality:{batch.QualityClass}";
        }

        private static string Classify(int score)
        {
            if (score < 45) return ExtrusionQuality.Rejected;
            if (score < 65) return ExtrusionQuality.Utility;
            if (score < 82) return ExtrusionQuality.HighPressure;
            return ExtrusionQuality.Premium;
        }

        /// <summary>Deterministic fallback defect roll when no RNG is injected.</summary>
        private static bool StableDefect(string batchId, int defectRiskBp)
        {
            if (defectRiskBp <= 0) return false;
            uint h = 2166136261u;
            foreach (char c in batchId) { h ^= c; h *= 16777619u; }
            return (h % 10000u) < (uint)defectRiskBp;
        }

        public ExtrusionBatch? FindBatch(string batchId)
            => _state.Batches.Find(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal));

        private static int ClampBp(int v) => Math.Max(0, Math.Min(100, v));
        private static double Clamp01(double v) => Math.Max(0.0, Math.Min(1.0, v));

        public HydraulicExtrusionState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<HydraulicExtrusionState>(s.Serialize(_state)) ?? new HydraulicExtrusionState();
        }

        public void RestoreState(HydraulicExtrusionState? saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            _state = s.Deserialize<HydraulicExtrusionState>(s.Serialize(saved)) ?? new HydraulicExtrusionState();
            if (string.IsNullOrEmpty(_state.SystemId)) _state.SystemId = SystemId;
        }
    }
}
