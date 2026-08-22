using System;
using System.Collections.Generic;

using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class DecontaminationState
    {
        public string systemId = DecontaminationSystem.SystemId;
        public List<DeconCase> queue = new List<DeconCase>();
        public DeconCase? activeCase;
        public bool shelterContaminated;
        public float shelterContaminationLevel;
        public List<DeconIncident> incidentLog = new List<DeconIncident>();
    }

    [Serializable]
    public sealed class DeconCase
    {
        public string caseId = string.Empty;
        public string survivorId = string.Empty;
        public string gearId = string.Empty;
        public float surfaceContamination;         // 0-1 (surface dust only, NOT lifetime dose)
        public float radiationDoseBeforeDecon;
        public DeconStatus status;
        public float progress;
        public int queuedDay = -1;
        public int startDay = -1;
        public int completeDay = -1;
        public bool bypassed;
        public string outcome = string.Empty;
    }

    public enum DeconStatus { Queued, InProgress, Complete, Bypassed, Failed }

    [Serializable]
    public sealed class DeconIncident
    {
        public int day;
        public string caseId = string.Empty;
        public string description = string.Empty;
    }

    public sealed class DecontaminationSystem
    {
        public const string SystemId = "decontamination";
        private DecontaminationState _state = new DecontaminationState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly RadiationSystem _radiation;
        private readonly Inventory.Inventory _inventory;
        private readonly AirlockSecuritySystem _airlock;
        private readonly StartingLevelSystem _startingLevel;
        private int _currentDay;

        public DecontaminationState State => _state;
        public bool HasActiveCase => _state.activeCase != null && _state.activeCase.status == DeconStatus.InProgress;
        public event Action<DeconCase> OnCaseCompleted;
        public event Action OnDeconChanged;

        public DecontaminationSystem(
            ISeededRng rng,
            RadiationSystem radiation,
            Inventory.Inventory inventory,
            AirlockSecuritySystem airlock,
            StartingLevelSystem startingLevel,
            ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _radiation = radiation ?? throw new ArgumentNullException(nameof(radiation));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _airlock = airlock ?? throw new ArgumentNullException(nameof(airlock));
            _startingLevel = startingLevel ?? throw new ArgumentNullException(nameof(startingLevel));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult Enqueue(string survivorId, string gearId, float surfaceContamination)
        {
            var caseId = $"decon_{_currentDay}_{survivorId}";
            if (_state.queue.Exists(c => c.caseId == caseId))
                return ActionResult.Blocked("already_queued", "decon.already_queued");

            var deconCase = new DeconCase
            {
                caseId = caseId, survivorId = survivorId, gearId = gearId,
                surfaceContamination = Math.Clamp(surfaceContamination, 0f, 1f),
                radiationDoseBeforeDecon = _radiation.GetDosimeter(survivorId)?.CurrentReading ?? 0f,
                status = DeconStatus.Queued, queuedDay = _currentDay
            };
            _state.queue.Add(deconCase);
            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.enqueued");
        }

        public ActionResult ProcessQueue()
        {
            if (_state.activeCase != null && _state.activeCase.status == DeconStatus.InProgress)
                return ActionResult.Blocked("active_case", "decon.active_case");

            if (_state.queue.Count == 0)
                return ActionResult.Blocked("empty_queue", "decon.empty_queue");

            // Atomic resource consumption: clean water + soap
            if (_inventory.CountById("water_clean") < 1)
                return ActionResult.Blocked("no_water", "decon.no_water");
            if (_inventory.CountById("soap") < 1)
                return ActionResult.Blocked("no_soap", "decon.no_soap");

            _inventory.RemoveById("water_clean", 1);
            _inventory.RemoveById("soap", 1);

            var next = _state.queue[0];
            next.status = DeconStatus.InProgress;
            next.startDay = _currentDay;
            _state.activeCase = next;
            _state.queue.RemoveAt(0);

            // Route to airlock
            _airlock.VisitorArrives(next.survivorId, "decon_subject");

            OnDeconChanged?.Invoke();
            return ActionResult.Success("decon.processing",
                new Dictionary<string, double> { { "queue_position", _state.queue.Count } });
        }

        public ActionResult CompleteCycle(bool safeRelease)
        {
            if (_state.activeCase == null)
                return ActionResult.Blocked("no_active", "decon.no_active");

            var c = _state.activeCase;
            if (c.status != DeconStatus.InProgress)
                return ActionResult.Blocked("not_in_progress", "decon.not_in_progress");

            if (safeRelease)
            {
                c.surfaceContamination = Math.Max(0, c.surfaceContamination - 0.8f);
                c.status = DeconStatus.Complete;
                c.completeDay = _currentDay;
                c.outcome = "decontaminated";

                // Reduce shelter air contamination slightly
                _state.shelterContaminationLevel = Math.Max(0, _state.shelterContaminationLevel - 0.05f);
                if (_state.shelterContaminationLevel == 0)
                    _state.shelterContaminated = false;
            }
            else
            {
                c.status = DeconStatus.Bypassed;
                c.bypassed = true;
                c.outcome = "bypassed";
                c.surfaceContamination = Math.Max(0, c.surfaceContamination - 0.1f);

                // Shelter contamination consequence
                _state.shelterContaminationLevel = Math.Min(1f, _state.shelterContaminationLevel + 0.1f);
                _state.shelterContaminated = true;

                var incident = new DeconIncident
                {
                    day = _currentDay, caseId = c.caseId,
                    description = $"{c.survivorId} bypassed decon — shelter contamination increased"
                };
                _state.incidentLog.Add(incident);
            }

            _log.Info($"[Decon] case {c.caseId}: {c.outcome} (surface={c.surfaceContamination:F2})");
            OnCaseCompleted?.Invoke(c);
            _state.activeCase = null;
            OnDeconChanged?.Invoke();
            return ActionResult.Success($"decon.{c.outcome}",
                new Dictionary<string, double> { { "surface_contamination", c.surfaceContamination } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            // Passive shelter contamination decay
            if (_state.shelterContaminated && _state.shelterContaminationLevel > 0)
            {
                _state.shelterContaminationLevel = Math.Max(0, _state.shelterContaminationLevel - 0.01f);
                if (_state.shelterContaminationLevel == 0)
                    _state.shelterContaminated = false;
            }
        }

        public DecontaminationState CaptureState() => _state;
        public void RestoreState(DecontaminationState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnDeconChanged?.Invoke();
        }
    }
}
