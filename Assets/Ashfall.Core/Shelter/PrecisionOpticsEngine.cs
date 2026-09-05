using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum OpticStageKind
    {
        RoughGrind,
        FineGrind,
        Polish,
        FigureTest,
        CorrectivePolish,
        Completed
    }

    [Serializable]
    public sealed class OpticStageDef
    {
        public string stage = string.Empty;
        public float work_units = 5.0f;
        public float quality_gain = 0.2f;
    }

    [Serializable]
    public sealed class OpticRecipeDef
    {
        public string optic_recipe_id = string.Empty;
        public string display_name = string.Empty;
        public string blank_item_id = "item_cast_borosilicate_glass_blank";
        public List<OpticStageDef> stages = new List<OpticStageDef>();
        public float max_quality = 0.85f;
        public List<string> required_tool_tags = new List<string>();
        public string target_system = string.Empty;
    }

    [Serializable]
    public sealed class PrecisionOpticsCatalog
    {
        public int schema_version = 1;
        public List<OpticRecipeDef> recipes = new List<OpticRecipeDef>();
    }

    [Serializable]
    public sealed class OpticWorkpiece
    {
        public string recipeId = string.Empty;
        public string displayName = string.Empty;
        public int currentStageIndex;
        public float stageProgress;
        public float accumulatedQuality;
        public float figureAberration = 0.25f; // wave error fraction, lower is better
        public bool isCompleted;
        public int defectCount;
    }

    [Serializable]
    public sealed class PrecisionOpticsState
    {
        public OpticWorkpiece? activeWorkpiece;
        public int completedOpticsCount;
        public float totalWorkUnitsExpended;
        public bool foucaultRigInstalled;
        public bool pitchLapAvailable = true;
    }

    public static class PrecisionOpticsCatalogLoader
    {
        public const string DefaultFileName = "precision_optics_catalog.json";

        public static PrecisionOpticsCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[PrecisionOptics] catalog not found at {path}");
                return new PrecisionOpticsCatalog();
            }

            try
            {
                string text = fileIO.ReadAllText(path);
                var cat = json.Deserialize<PrecisionOpticsCatalog>(text);
                return cat ?? new PrecisionOpticsCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[PrecisionOptics] failed loading catalog: {ex.Message}");
                return new PrecisionOpticsCatalog();
            }
        }
    }

    public sealed class PrecisionOpticsEngine
    {
        public const string SystemId = "precision_optics";
        public const string ItemGlassBlank = "item_cast_borosilicate_glass_blank";
        public const string ItemPolishingRouge = "item_cerium_oxide_polishing_rouge";
        public const string ItemPitchLap = "item_optical_pitch_lap";
        public const string ItemFoucaultRig = "item_foucault_tester_rig";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly ILog? _log;

        private PrecisionOpticsCatalog _catalog = new PrecisionOpticsCatalog();
        private PrecisionOpticsState _state = new PrecisionOpticsState();

        public event Action<PrecisionOpticsState>? OnStateChanged;
        public event Action<OpticWorkpiece>? OnWorkpieceStarted;
        public event Action<OpticWorkpiece, string>? OnStageCompleted;
        public event Action<OpticWorkpiece>? OnWorkpieceCompleted;

        public PrecisionOpticsState State => _state;
        public PrecisionOpticsCatalog Catalog => _catalog;

        public PrecisionOpticsEngine(Inventory.Inventory inventory, ISeededRng rng, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log;
        }

        public void LoadCatalog(PrecisionOpticsCatalog catalog)
        {
            _catalog = catalog ?? new PrecisionOpticsCatalog();
        }

        public ActionResult StartWorkpiece(string recipeId)
        {
            if (_state.activeWorkpiece != null)
                return ActionResult.Blocked("workpiece_in_progress", "An optical workpiece is already mounted on the bench.");

            var recipe = _catalog.recipes.FirstOrDefault(r => r.optic_recipe_id == recipeId);
            if (recipe == null)
                return ActionResult.Failed("unknown_recipe", $"No optics recipe found for ID: {recipeId}");

            string blankId = string.IsNullOrEmpty(recipe.blank_item_id) ? ItemGlassBlank : recipe.blank_item_id;
            if (_inventory.CountById(blankId) < 1)
                return ActionResult.Blocked("missing_blank", $"Requires 1x {blankId} to grind.");

            if (!_inventory.TryConsumeById(blankId, 1))
                return ActionResult.Failed("consume_failed", $"Failed consuming glass blank {blankId}.");

            var workpiece = new OpticWorkpiece
            {
                recipeId = recipe.optic_recipe_id,
                displayName = recipe.display_name,
                currentStageIndex = 0,
                stageProgress = 0f,
                accumulatedQuality = 0.1f,
                figureAberration = 0.30f,
                isCompleted = false,
                defectCount = 0
            };

            _state.activeWorkpiece = workpiece;
            _log?.Info($"[PrecisionOptics] Started optical blank grinding for '{recipe.display_name}'.");
            OnWorkpieceStarted?.Invoke(workpiece);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("optic_workpiece_started");
        }

        public ActionResult AdvanceWork(float workUnits, float workerSkillModifier = 1.0f)
        {
            var wp = _state.activeWorkpiece;
            if (wp == null)
                return ActionResult.Blocked("no_active_workpiece", "No optical workpiece on the bench.");

            if (wp.isCompleted)
                return ActionResult.Blocked("already_completed", "Active workpiece is already completed and ready to finalize.");

            var recipe = _catalog.recipes.FirstOrDefault(r => r.optic_recipe_id == wp.recipeId);
            if (recipe == null || wp.currentStageIndex >= recipe.stages.Count)
                return ActionResult.Failed("invalid_recipe_state", "Workpiece recipe stages invalid.");

            var currentStage = recipe.stages[wp.currentStageIndex];

            // If polishing or figure testing, require consumables or tools if applicable
            if (currentStage.stage.Contains("polish", StringComparison.OrdinalIgnoreCase))
            {
                if (_inventory.CountById(ItemPolishingRouge) > 0)
                {
                    // Consumes rouge with a small chance per work unit
                    if (_rng.NextDouble() < 0.15)
                    {
                        _inventory.TryConsumeById(ItemPolishingRouge, 1);
                    }
                }
            }

            float skillMult = Math.Clamp(workerSkillModifier, 0.5f, 2.0f);
            float effectiveWork = workUnits * skillMult;
            wp.stageProgress += effectiveWork;
            _state.totalWorkUnitsExpended += effectiveWork;

            if (wp.stageProgress >= currentStage.work_units)
            {
                // Stage complete
                float variance = (float)(_rng.NextDouble() * 0.04 - 0.02); // -0.02 to +0.02
                float gain = Math.Max(0.01f, currentStage.quality_gain * (1.0f + (skillMult - 1.0f) * 0.25f) + variance);
                wp.accumulatedQuality = Math.Min(recipe.max_quality, wp.accumulatedQuality + gain);
                wp.figureAberration = Math.Max(0.02f, wp.figureAberration - (gain * 0.5f));

                string completedStageName = currentStage.stage;
                wp.currentStageIndex++;
                wp.stageProgress = 0f;

                if (wp.currentStageIndex >= recipe.stages.Count)
                {
                    wp.isCompleted = true;
                    _log?.Info($"[PrecisionOptics] Workpiece '{wp.displayName}' fully finished! Quality: {wp.accumulatedQuality:P1}");
                    OnWorkpieceCompleted?.Invoke(wp);
                }
                else
                {
                    _log?.Info($"[PrecisionOptics] Workpiece '{wp.displayName}' finished stage '{completedStageName}'. Next: stage {wp.currentStageIndex}.");
                    OnStageCompleted?.Invoke(wp, completedStageName);
                }
            }

            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("optic_work_advanced");
        }

        public ActionResult TestFigureWithFoucault()
        {
            var wp = _state.activeWorkpiece;
            if (wp == null)
                return ActionResult.Blocked("no_active_workpiece", "No optical workpiece on the bench.");

            bool hasRig = _state.foucaultRigInstalled || _inventory.CountById(ItemFoucaultRig) > 0;
            if (!hasRig)
                return ActionResult.Blocked("missing_rig", "Foucault Knife-Edge Test Rig required to inspect figure wavefront.");

            // Knife-edge test reveals surface error and allows fine corrective figuring
            float measurementNoise = (float)(_rng.NextDouble() * 0.01);
            wp.figureAberration = Math.Max(0.01f, wp.figureAberration - 0.05f + measurementNoise);
            wp.accumulatedQuality = Math.Min(1.0f, wp.accumulatedQuality + 0.05f);

            _log?.Info($"[PrecisionOptics] Foucault knife-edge test complete. Residual wavefront error: λ/{1.0f / wp.figureAberration:F1}");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("figure_tested");
        }

        public ActionResult CompleteOptic(string? outputItemId = null)
        {
            var wp = _state.activeWorkpiece;
            if (wp == null)
                return ActionResult.Blocked("no_active_workpiece", "No active workpiece.");

            if (!wp.isCompleted)
                return ActionResult.Blocked("workpiece_incomplete", "Workpiece has not completed all manufacturing stages.");

            string finalItemId = !string.IsNullOrEmpty(outputItemId) ? outputItemId : wp.recipeId;
            _inventory.AddById(finalItemId, 1);
            _state.completedOpticsCount++;
            _state.activeWorkpiece = null;

            _log?.Info($"[PrecisionOptics] Produced precision optical assembly '{finalItemId}' (quality: {wp.accumulatedQuality:P1}).");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("optic_completed");
        }

        public ActionResult ScrapWorkpiece()
        {
            var wp = _state.activeWorkpiece;
            if (wp == null)
                return ActionResult.Blocked("no_active_workpiece", "No active workpiece to scrap.");

            _state.activeWorkpiece = null;
            _log?.Warn($"[PrecisionOptics] Scrapped workpiece '{wp.displayName}'.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("workpiece_scrapped");
        }

        public ActionResult InstallFoucaultRig()
        {
            if (_state.foucaultRigInstalled)
                return ActionResult.Blocked("already_installed", "Foucault testing rig is already installed.");

            if (_inventory.CountById(ItemFoucaultRig) < 1)
                return ActionResult.Blocked("missing_item", $"Requires 1x {ItemFoucaultRig}.");

            _inventory.TryConsumeById(ItemFoucaultRig, 1);
            _state.foucaultRigInstalled = true;
            _log?.Info("[PrecisionOptics] Installed Foucault Knife-Edge Test Rig on optics bench.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("rig_installed");
        }

        public PrecisionOpticsState CaptureState()
        {
            return new PrecisionOpticsState
            {
                activeWorkpiece = _state.activeWorkpiece == null ? null : new OpticWorkpiece
                {
                    recipeId = _state.activeWorkpiece.recipeId,
                    displayName = _state.activeWorkpiece.displayName,
                    currentStageIndex = _state.activeWorkpiece.currentStageIndex,
                    stageProgress = _state.activeWorkpiece.stageProgress,
                    accumulatedQuality = _state.activeWorkpiece.accumulatedQuality,
                    figureAberration = _state.activeWorkpiece.figureAberration,
                    isCompleted = _state.activeWorkpiece.isCompleted,
                    defectCount = _state.activeWorkpiece.defectCount
                },
                completedOpticsCount = _state.completedOpticsCount,
                totalWorkUnitsExpended = _state.totalWorkUnitsExpended,
                foucaultRigInstalled = _state.foucaultRigInstalled,
                pitchLapAvailable = _state.pitchLapAvailable
            };
        }

        public void RestoreState(PrecisionOpticsState? state)
        {
            if (state == null) return;
            _state = new PrecisionOpticsState
            {
                activeWorkpiece = state.activeWorkpiece == null ? null : new OpticWorkpiece
                {
                    recipeId = state.activeWorkpiece.recipeId,
                    displayName = state.activeWorkpiece.displayName,
                    currentStageIndex = state.activeWorkpiece.currentStageIndex,
                    stageProgress = state.activeWorkpiece.stageProgress,
                    accumulatedQuality = state.activeWorkpiece.accumulatedQuality,
                    figureAberration = state.activeWorkpiece.figureAberration,
                    isCompleted = state.activeWorkpiece.isCompleted,
                    defectCount = state.activeWorkpiece.defectCount
                },
                completedOpticsCount = state.completedOpticsCount,
                totalWorkUnitsExpended = state.totalWorkUnitsExpended,
                foucaultRigInstalled = state.foucaultRigInstalled,
                pitchLapAvailable = state.pitchLapAvailable
            };
            OnStateChanged?.Invoke(_state);
        }
    }
}
