// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum PrisonerStatus
    {
        Detained = 0,
        Interrogating = 1,
        PenalLabor = 2,
        Paroled = 3,
        Escaped = 4,
        Deceased = 5
    }

    public enum InterrogationApproach
    {
        RapportBuilding = 0,
        MaterialBargaining = 1,
        FirmPressure = 2,
        SensoryIsolation = 3
    }

    public enum PenalShiftKind
    {
        SlurryPumping = 0,
        RubbleClearing = 1,
        FilterScrubbing = 2
    }

    [Serializable]
    public sealed class PrisonerRecord
    {
        public string PrisonerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ArchetypeId { get; set; } = string.Empty;
        public string FactionOrigin { get; set; } = string.Empty;
        public PrisonerStatus Status { get; set; }
        public float Resistance { get; set; }
        public float Hostility { get; set; }
        public float Compliance { get; set; }
        public float Fatigue { get; set; }
        public List<string> ExtractedTopics { get; set; } = new List<string>();
        public int DaysInDetention { get; set; }
        public PenalShiftKind? AssignedShift { get; set; }
    }

    [Serializable]
    public sealed class ShelterPrisonerState
    {
        public string SystemId { get; set; } = ShelterPrisonerSystem.SystemId;
        public List<PrisonerRecord> Prisoners { get; set; } = new List<PrisonerRecord>();
        public List<string> AssignedGuards { get; set; } = new List<string>();
        public int MaxCapacity { get; set; } = 4;
        public int TotalParoled { get; set; }
        public int TotalEscaped { get; set; }
        public int TotalLaborShiftsCompleted { get; set; }
        public int NextPrisonerSeq { get; set; } = 1;
    }

    public sealed class ShelterPrisonerSystem
    {
        public const string SystemId = "shelter_prisoners";

        private ShelterPrisonerState _state = new ShelterPrisonerState();
        private readonly ISeededRng _rng;
        private readonly Inventory.Inventory _inventory;
        private readonly CaptiveInterrogationCatalog _catalog;
        private readonly ILog _log;
        private int _currentDay;

        public ShelterPrisonerState State => _state;
        public int PrisonerCount => _state.Prisoners.Count;
        public int TotalParoled => _state.TotalParoled;
        public int TotalEscaped => _state.TotalEscaped;

        public event Action<PrisonerRecord>? OnPrisonerCaptured;
        public event Action<PrisonerRecord, InterrogationTopicDef>? OnTopicExtracted;
        public event Action<PrisonerRecord>? OnPrisonerParoled;
        public event Action<PrisonerRecord>? OnPrisonerEscaped;

        public ShelterPrisonerSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            CaptiveInterrogationCatalog catalog,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult CapturePrisoner(string archetypeId, string name, int currentDay)
        {
            var arch = _catalog.GetArchetype(archetypeId);
            if (arch == null)
                return ActionResult.Blocked("unknown_archetype", "captive.unknown_archetype");

            if (_state.Prisoners.Count >= _state.MaxCapacity)
                return ActionResult.Blocked("detention_full", "captive.detention_full");

            _state.NextPrisonerSeq++;
            var prisoner = new PrisonerRecord
            {
                PrisonerId = $"captive_{_state.NextPrisonerSeq}",
                Name = string.IsNullOrEmpty(name) ? arch.display_name : name,
                ArchetypeId = archetypeId,
                FactionOrigin = arch.faction_origin,
                Status = PrisonerStatus.Detained,
                Resistance = arch.base_resistance,
                Hostility = arch.base_hostility,
                Compliance = 0f,
                Fatigue = 0f,
                DaysInDetention = 0
            };

            _state.Prisoners.Add(prisoner);
            _log.Info($"[ShelterPrisoner] Detained captive {prisoner.Name} ({prisoner.PrisonerId}) from {prisoner.FactionOrigin}");
            OnPrisonerCaptured?.Invoke(prisoner);
            return ActionResult.Success("captive.captured");
        }

        public ActionResult AssignGuards(IEnumerable<string> guardSurvivorIds)
        {
            _state.AssignedGuards.Clear();
            if (guardSurvivorIds != null)
            {
                _state.AssignedGuards.AddRange(guardSurvivorIds);
            }
            return ActionResult.Success("captive.guards_assigned");
        }

        public ActionResult Interrogate(string prisonerId, string topicId, InterrogationApproach approach, string interrogatorSurvivorId)
        {
            var prisoner = _state.Prisoners.Find(p => p.PrisonerId == prisonerId);
            if (prisoner == null)
                return ActionResult.Blocked("not_found", "captive.not_found");

            if (prisoner.Status != PrisonerStatus.Detained && prisoner.Status != PrisonerStatus.Interrogating)
                return ActionResult.Blocked("invalid_status", "captive.cannot_interrogate");

            var topic = _catalog.GetTopic(topicId);
            if (topic == null)
                return ActionResult.Blocked("unknown_topic", "captive.unknown_topic");

            if (prisoner.ExtractedTopics.Contains(topicId))
                return ActionResult.Blocked("already_extracted", "captive.topic_already_extracted");

            prisoner.Status = PrisonerStatus.Interrogating;

            // Interrogation effects based on approach
            float resReduction = 0f;
            float compGain = 0f;
            float hostDelta = 0f;

            switch (approach)
            {
                case InterrogationApproach.RapportBuilding:
                    resReduction = 15f;
                    compGain = 20f;
                    hostDelta = -15f; // Decreases hostility
                    break;
                case InterrogationApproach.MaterialBargaining:
                    resReduction = 25f;
                    compGain = 15f;
                    hostDelta = -5f;
                    break;
                case InterrogationApproach.FirmPressure:
                    resReduction = 35f;
                    compGain = 10f;
                    hostDelta = 10f; // Increases hostility
                    break;
                case InterrogationApproach.SensoryIsolation:
                    resReduction = 45f;
                    compGain = 5f;
                    hostDelta = 25f; // High hostility & trauma
                    break;
            }

            prisoner.Resistance = Math.Max(0f, prisoner.Resistance - resReduction);
            prisoner.Compliance = Math.Min(100f, prisoner.Compliance + compGain);
            prisoner.Hostility = Math.Clamp(prisoner.Hostility + hostDelta, 0f, 100f);

            // Check if topic is successfully extracted
            if (prisoner.Resistance <= topic.resistance_cost || prisoner.Compliance >= 60f)
            {
                prisoner.ExtractedTopics.Add(topicId);

                // Reward delivered if configured
                if (!string.IsNullOrEmpty(topic.reward_item_id))
                {
                    _inventory.AddById(topic.reward_item_id, 1);
                }

                _log.Info($"[ShelterPrisoner] Successfully extracted topic {topic.display_name} from {prisoner.Name}!");
                OnTopicExtracted?.Invoke(prisoner, topic);
                return ActionResult.Success("captive.topic_extracted");
            }

            return ActionResult.Success("captive.interrogation_progress");
        }

        public ActionResult AssignPenalLabor(string prisonerId, PenalShiftKind shift)
        {
            var prisoner = _state.Prisoners.Find(p => p.PrisonerId == prisonerId);
            if (prisoner == null)
                return ActionResult.Blocked("not_found", "captive.not_found");

            if (prisoner.Status == PrisonerStatus.Paroled || prisoner.Status == PrisonerStatus.Escaped)
                return ActionResult.Blocked("invalid_status", "captive.cannot_assign_labor");

            prisoner.Status = PrisonerStatus.PenalLabor;
            prisoner.AssignedShift = shift;

            _log.Info($"[ShelterPrisoner] Assigned {prisoner.Name} to penal labor: {shift}");
            return ActionResult.Success("captive.labor_assigned");
        }

        public ActionResult RelievePenalLabor(string prisonerId)
        {
            var prisoner = _state.Prisoners.Find(p => p.PrisonerId == prisonerId);
            if (prisoner == null)
                return ActionResult.Blocked("not_found", "captive.not_found");

            prisoner.Status = PrisonerStatus.Detained;
            prisoner.AssignedShift = null;
            return ActionResult.Success("captive.labor_relieved");
        }

        public ActionResult GrantParole(string prisonerId, out string grantedSurvivorId)
        {
            grantedSurvivorId = string.Empty;
            var prisoner = _state.Prisoners.Find(p => p.PrisonerId == prisonerId);
            if (prisoner == null)
                return ActionResult.Blocked("not_found", "captive.not_found");

            // Requirements for parole: high compliance, low hostility and resistance
            if (prisoner.Compliance < 60f || prisoner.Hostility > 30f)
                return ActionResult.Blocked("not_reformed", "captive.not_ready_for_parole");

            prisoner.Status = PrisonerStatus.Paroled;
            grantedSurvivorId = $"survivor_paroled_{prisoner.PrisonerId}";
            _state.Prisoners.Remove(prisoner);
            _state.TotalParoled++;

            _log.Info($"[ShelterPrisoner] Granted parole to {prisoner.Name}! Inducted as {grantedSurvivorId}");
            OnPrisonerParoled?.Invoke(prisoner);
            return ActionResult.Success("captive.paroled");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            float guardRatio = _state.Prisoners.Count > 0
                ? (float)_state.AssignedGuards.Count / _state.Prisoners.Count
                : 1f;

            for (int i = _state.Prisoners.Count - 1; i >= 0; i--)
            {
                var prisoner = _state.Prisoners[i];
                prisoner.DaysInDetention++;

                // If on penal labor: builds fatigue, increments completed shifts
                if (prisoner.Status == PrisonerStatus.PenalLabor)
                {
                    prisoner.Fatigue = Math.Min(100f, prisoner.Fatigue + 20f);
                    _state.TotalLaborShiftsCompleted++;

                    // Penal labor produces basic scrap
                    _inventory.AddById("item_travel_ration", 1);
                }
                else
                {
                    prisoner.Fatigue = Math.Max(0f, prisoner.Fatigue - 25f);
                }

                // Check escape risk if guard presence is inadequate
                if (guardRatio < 0.5f && prisoner.Hostility > 50f)
                {
                    double escapeRoll = _rng.NextDouble();
                    if (escapeRoll < 0.20) // 20% escape chance when under-guarded and hostile
                    {
                        prisoner.Status = PrisonerStatus.Escaped;
                        _state.Prisoners.RemoveAt(i);
                        _state.TotalEscaped++;
                        _log.Warn($"[ShelterPrisoner] Captive {prisoner.Name} ({prisoner.PrisonerId}) ESCAPED due to low guard presence!");
                        OnPrisonerEscaped?.Invoke(prisoner);
                        continue;
                    }
                }
            }
        }

        public PrisonerRecord? GetPrisoner(string prisonerId)
        {
            return _state.Prisoners.Find(p => p.PrisonerId == prisonerId);
        }

        public ShelterPrisonerState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<ShelterPrisonerState>(json) ?? new ShelterPrisonerState();
        }

        public void RestoreState(ShelterPrisonerState saved)
        {
            if (saved == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<ShelterPrisonerState>(json) ?? new ShelterPrisonerState();
        }
    }
}
