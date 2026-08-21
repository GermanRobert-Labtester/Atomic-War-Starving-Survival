using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class AutopsyState
    {
        public string systemId = AutopsySystem.SystemId;
        public List<AutopsyCase> cases = new List<AutopsyCase>();
        public List<string> completedSpecimenIds = new List<string>();
    }

    [Serializable]
    public sealed class AutopsyProcedure
    {
        public string procedure_id = string.Empty;
        public string display_name = string.Empty;
        public List<string> requiredTools = new List<string>();
        public List<string> requiredConsumables = new List<string>();
        public float airborneRisk = 0.1f;
        public float pathogenRisk = 0.05f;
        public int procedureHours = 4;
        public List<string> possibleFindings = new List<string>();
        public List<string> researchUnlocks = new List<string>();
    }

    [Serializable]
    public sealed class AutopsyCase
    {
        public string caseId = string.Empty;
        public string specimenId = string.Empty;   // deceased survivor ID
        public string procedureId = string.Empty;
        public string assignedMedicId = string.Empty;
        public int dayStarted = -1;
        public float progressHours;
        public AutopsyStatus status;
        public string finding = string.Empty;
        public bool containmentBreach;
        public List<string> sideEffects = new List<string>();
    }

    public enum AutopsyStatus { Queued, InProgress, Complete, Failed, ContainmentBreach }

    public sealed class AutopsySystem
    {
        public const string SystemId = "autopsy";
        private AutopsyState _state = new AutopsyState();
        private readonly Dictionary<string, AutopsyProcedure> _catalog = new Dictionary<string, AutopsyProcedure>(StringComparer.Ordinal);
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory _inventory;
        private readonly RadiationSystem _radiation;
        private readonly VentilationSystem _ventilation;
        private readonly ResearchSystem _research;
        private readonly MedicalWardSystem _medical;
        private int _currentDay;

        public AutopsyState State => _state;
        public event Action<AutopsyCase> OnCaseCompleted;
        public event Action OnAutopsyChanged;

        public AutopsySystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            RadiationSystem radiation,
            VentilationSystem ventilation,
            ResearchSystem research,
            MedicalWardSystem medical,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _radiation = radiation ?? throw new ArgumentNullException(nameof(radiation));
            _ventilation = ventilation ?? throw new ArgumentNullException(nameof(ventilation));
            _research = research ?? throw new ArgumentNullException(nameof(research));
            _medical = medical ?? throw new ArgumentNullException(nameof(medical));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(List<AutopsyProcedure> procedures)
        {
            if (procedures == null) return;
            _catalog.Clear();
            foreach (var p in procedures)
                if (!string.IsNullOrEmpty(p.procedure_id))
                    _catalog[p.procedure_id] = p;
        }

        public ActionResult QueueAutopsy(string specimenId, string procedureId, string medicId)
        {
            if (_state.completedSpecimenIds.Contains(specimenId))
                return ActionResult.Blocked("already_processed", "autopsy.already_processed");

            if (!_catalog.TryGetValue(procedureId, out var procedure))
                return ActionResult.Failed("unknown_procedure", "autopsy.unknown_procedure");

            // Reserve tools and consumables
            foreach (var tool in procedure.requiredTools)
            {
                if (_inventory.CountById(tool) < 1)
                    return ActionResult.Blocked("missing_tool", "autopsy.missing_tool");
            }
            foreach (var consumable in procedure.requiredConsumables)
            {
                if (_inventory.CountById(consumable) < 1)
                    return ActionResult.Blocked("missing_consumable", "autopsy.missing_consumable");
            }

            var case_ = new AutopsyCase
            {
                caseId = $"autopsy_{_currentDay}_{specimenId}",
                specimenId = specimenId, procedureId = procedureId,
                assignedMedicId = medicId, dayStarted = _currentDay,
                status = AutopsyStatus.Queued
            };
            _state.cases.Add(case_);
            OnAutopsyChanged?.Invoke();
            return ActionResult.Success("autopsy.queued");
        }

        public ActionResult BeginAutopsy(string caseId)
        {
            var case_ = _state.cases.Find(c => c.caseId == caseId);
            if (case_ == null) return ActionResult.Failed("unknown_case", "autopsy.unknown_case");
            if (case_.status != AutopsyStatus.Queued) return ActionResult.Blocked("not_queued", "autopsy.not_queued");

            // Consume tools and consumables
            if (!_catalog.TryGetValue(case_.procedureId, out var procedure)) return ActionResult.Failed("missing_procedure", "autopsy.missing_procedure");

            foreach (var tool in procedure.requiredTools)
                _inventory.RemoveById(tool, 1);
            foreach (var consumable in procedure.requiredConsumables)
                _inventory.RemoveById(consumable, 1);

            case_.status = AutopsyStatus.InProgress;
            OnAutopsyChanged?.Invoke();
            return ActionResult.Success("autopsy.started");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var case_ in _state.cases)
            {
                if (case_.status != AutopsyStatus.InProgress) continue;

                if (!_catalog.TryGetValue(case_.procedureId, out var procedure)) continue;

                case_.progressHours += 8f;

                // Airborne/pathogen risk
                if (_rng.NextDouble() < procedure.airborneRisk)
                {
                    case_.containmentBreach = true;
                    _ventilation.RegisterSource(new VentilationSource
                    {
                        sourceId = $"autopsy_{case_.caseId}",
                        smokeOutputPerDay = 0f,
                        coOutputPerDay = 0f,
                        requiresExhaust = true
                    });
                    _log.Warn($"[Autopsy] containment risk in {case_.caseId}");
                }

                if (case_.progressHours >= procedure.procedureHours)
                {
                    case_.status = AutopsyStatus.Complete;
                    _state.completedSpecimenIds.Add(case_.specimenId);

                    // Random finding
                    if (procedure.possibleFindings.Count > 0)
                    {
                        int idx = _rng.Next(0, procedure.possibleFindings.Count);
                        case_.finding = procedure.possibleFindings[idx];
                    }

                    // Unlock research
                    foreach (var unlock in procedure.researchUnlocks)
                        _research.UnlockManual(unlock);

                    _log.Info($"[Autopsy] {case_.caseId} complete: {case_.finding}");
                    OnCaseCompleted?.Invoke(case_);
                }
            }

            _state.cases.RemoveAll(c => c.status == AutopsyStatus.Complete || c.status == AutopsyStatus.Failed);
            OnAutopsyChanged?.Invoke();
        }

        public List<AutopsyCase> GetActiveCases() => _state.cases.FindAll(c => c.status == AutopsyStatus.InProgress);

        public AutopsyState CaptureState() => _state;
        public void RestoreState(AutopsyState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnAutopsyChanged?.Invoke();
        }
    }
}
