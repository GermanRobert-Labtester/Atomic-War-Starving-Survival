using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class MentalHealthState
    {
        public string systemId = MentalHealthCrisisSystem.SystemId;
        public List<CrisisCase> activeCases = new List<CrisisCase>();
        public List<CrisisCase> resolvedCases = new List<CrisisCase>();
        public int wardCapacity = 2;
        public int currentOccupancy;
    }

    [Serializable]
    public sealed class CrisisCase
    {
        public string caseId = string.Empty;
        public string survivorId = string.Empty;
        public CrisisProfile profile;
        public CrisisAcuity acuity;
        public int dayStarted = -1;
        public int dayResolved = -1;
        public CrisisStatus status;
        public string assignedCaregiverId = string.Empty;
        public string intervention = string.Empty;
        public float stressInput;
        public float recoveryProgress;
        public List<string> sideEffects = new List<string>();
    }

    public enum CrisisAcuity { Mild, Moderate, Severe, Critical }
    public enum CrisisStatus { Active, InTreatment, Recovering, Recovered, Chronic }
    public enum CrisisProfile { AcuteStress, SomaticFlashback, GuiltInsomnia, ChemicalWithdrawal, IsolationParanoia }

    public sealed class MentalHealthCrisisSystem
    {
        public const string SystemId = "mental_health";
        private MentalHealthState _state = new MentalHealthState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly NeedsSystem _needs;
        private readonly MedicalWardSystem _medical;
        private readonly ChemicalDependencySystem _dependency;
        private readonly DutyRosterSystem _roster;
        private int _currentDay;

        public MentalHealthState State => _state;
        public event Action<CrisisCase> OnCrisisResolved;
        public event Action OnMentalHealthChanged;

        public MentalHealthCrisisSystem(
            ISeededRng rng,
            NeedsSystem needs,
            MedicalWardSystem medical,
            ChemicalDependencySystem dependency,
            DutyRosterSystem roster,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _needs = needs ?? throw new ArgumentNullException(nameof(needs));
            _medical = medical ?? throw new ArgumentNullException(nameof(medical));
            _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
            _roster = roster ?? throw new ArgumentNullException(nameof(roster));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult TriggerCrisis(string survivorId, float stressInput, CrisisProfile profile)
        {
            if (_state.activeCases.Exists(c => c.survivorId == survivorId && c.status != CrisisStatus.Recovered && c.status != CrisisStatus.Chronic))
                return ActionResult.Blocked("active_case", "mental.active_case");

            if (_state.currentOccupancy >= _state.wardCapacity)
                return ActionResult.Blocked("ward_full", "mental.ward_full");

            var acuity = stressInput > 80f ? CrisisAcuity.Critical :
                         stressInput > 60f ? CrisisAcuity.Severe :
                         stressInput > 40f ? CrisisAcuity.Moderate : CrisisAcuity.Mild;

            var crisis = new CrisisCase
            {
                caseId = $"crisis_{_currentDay}_{survivorId}",
                survivorId = survivorId, profile = profile, acuity = acuity,
                dayStarted = _currentDay, stressInput = stressInput,
                status = CrisisStatus.Active
            };
            _state.activeCases.Add(crisis);
            _state.currentOccupancy++;

            // Remove from duty roster
            string activeRole = _roster.GetRoleOf(survivorId)!;
            if (!string.IsNullOrEmpty(activeRole))
                _roster.Assign(activeRole, string.Empty);

            _log.Warn($"[MentalHealth] crisis: {survivorId} — {profile} ({acuity})");
            OnMentalHealthChanged?.Invoke();
            return ActionResult.Success("mental.crisis_triggered");
        }

        public ActionResult BeginTreatment(string caseId, string caregiverId, string intervention)
        {
            var crisis = _state.activeCases.Find(c => c.caseId == caseId);
            if (crisis == null) return ActionResult.Failed("unknown_case", "mental.unknown_case");
            if (crisis.status != CrisisStatus.Active) return ActionResult.Blocked("not_active", "mental.not_active");

            // Bug-09: caregiver must not currently hold a duty role — doing so
            // pulls a critical shift worker away from their assignment.
            if (!string.IsNullOrEmpty(caregiverId) && _roster.GetRoleOf(caregiverId) != null)
                return ActionResult.Blocked("caregiver_busy", "mental.caregiver_busy");

            crisis.status = CrisisStatus.InTreatment;
            crisis.assignedCaregiverId = caregiverId;
            crisis.intervention = intervention;
            OnMentalHealthChanged?.Invoke();
            return ActionResult.Success("mental.treatment_started");
        }

        public const int ChronicThresholdDays = 14;

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var crisis in _state.activeCases)
            {
                if (crisis.status == CrisisStatus.InTreatment)
                {
                    // Recovery progress based on acuity and intervention
                    float recoveryRate = crisis.acuity switch
                    {
                        CrisisAcuity.Mild => 15f,
                        CrisisAcuity.Moderate => 10f,
                        CrisisAcuity.Severe => 5f,
                        CrisisAcuity.Critical => 2f,
                        _ => 5f
                    };
                    crisis.recoveryProgress += recoveryRate;

                    if (crisis.recoveryProgress >= 100f)
                    {
                        crisis.status = CrisisStatus.Recovered;
                        crisis.dayResolved = day;
                        _state.currentOccupancy--;
                        _state.resolvedCases.Add(crisis);

                        // Restore morale
                        _needs.Modify(crisis.survivorId, NeedKind.Morale, 10f);

                        _log.Info($"[MentalHealth] {crisis.survivorId} recovered from {crisis.profile}");
                        OnCrisisResolved?.Invoke(crisis);
                    }
                }
                else if (crisis.status == CrisisStatus.Active
                         && crisis.dayStarted >= 0
                         && day - crisis.dayStarted > ChronicThresholdDays)
                {
                    // Bug-05: a crisis left untreated past the threshold does not
                    // hold a ward bed forever. Transition to Chronic, archive
                    // into resolvedCases for history, and free occupancy. Do not
                    // grant the morale boost from the recovery branch (Chronic
                    // is not the same as recovered).
                    crisis.status = CrisisStatus.Chronic;
                    crisis.dayResolved = day;
                    _state.currentOccupancy--;
                    _state.resolvedCases.Add(crisis);
                    _log.Warn($"[MentalHealth] {crisis.survivorId} untreated for "
                              + $"{day - crisis.dayStarted} days → Chronic");
                    OnMentalHealthChanged?.Invoke();
                }
            }

            _state.activeCases.RemoveAll(c => c.status == CrisisStatus.Recovered || c.status == CrisisStatus.Chronic);
            OnMentalHealthChanged?.Invoke();
        }

        public bool IsInCrisis(string survivorId)
        {
            return _state.activeCases.Exists(c => c.survivorId == survivorId && c.status != CrisisStatus.Recovered && c.status != CrisisStatus.Chronic);
        }

        public bool IsEligibleForWork(string survivorId)
        {
            var crisis = _state.activeCases.Find(c => c.survivorId == survivorId);
            return crisis == null || crisis.status == CrisisStatus.Recovered || crisis.status == CrisisStatus.Chronic;
        }

        public MentalHealthState CaptureState() => _state;
        public void RestoreState(MentalHealthState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnMentalHealthChanged?.Invoke();
        }
    }
}
