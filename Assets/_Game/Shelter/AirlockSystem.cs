using System;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #128 — Physical airlock room: outside temp/rad, inside storage. Contamination gating.</summary>
    public class AirlockSystem
    {
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;
        public const string AirlockRoomId = "airlock";
        public const float AirlockContaminationSpike = 0.35f; // contamination spread when inner door opens without decon
        public const float DeconHours = 1f;

        private bool _airlockExists;
        private bool _innerDoorSealed = true;
        private float _airlockContamination;
        private bool _scavengerInAirlock;

        public bool Exists => _airlockExists;
        public bool InnerDoorSealed => _innerDoorSealed;
        public float Contamination => _airlockContamination;
        public bool ScavengerInAirlock => _scavengerInAirlock;

        public event Action OnAirlockBuilt;
        public event Action<float> OnContaminationSpike; // amount spread to bunker
        public event Action OnInnerDoorBreached; // scavenger opened inner door without decon

        public void BuildAirlock() { if (!_airlockExists) { _airlockExists = true; OnAirlockBuilt?.Invoke(); } }

        /// <summary>Scavenger enters airlock from outside. Must decon before inner door opens.</summary>
        public void ScavengerEnterAirlock(Survivors.Survivor sv)
        {
            _scavengerInAirlock = true;
            _airlockContamination = Mathf.Clamp01(_airlockContamination + 0.1f);
        }

        /// <summary>Open inner door without decon — contamination floods bunker.</summary>
        public float OpenInnerDoorUnsafe(Shelter shelter)
        {
            if (!_innerDoorSealed || !_scavengerInAirlock) return 0f;
            _innerDoorSealed = false;
            _scavengerInAirlock = false;
            float spike = _airlockContamination + AirlockContaminationSpike;
            _airlockContamination = 0f;
            if (shelter?.Rooms != null)
                for (int i = 0; i < shelter.Rooms.Count; i++)
                    if (shelter.Rooms[i] != null)
                        shelter.Rooms[i].AmbientContamination = Mathf.Clamp01(shelter.Rooms[i].AmbientContamination + spike);
            OnContaminationSpike?.Invoke(spike);
            OnInnerDoorBreached?.Invoke();
            return spike;
        }

        /// <summary>Decontaminate in airlock before opening inner door. Safe.</summary>
        public void DeconAndEnter(Survivors.Survivor sv, PersonalQuestSystem personalQuests = null)
        {
            _airlockContamination = 0f;
            _scavengerInAirlock = false;
            _innerDoorSealed = true;
            // Prompt #234 — Rad-Walker: contamination falls off without decon labor.
            if (personalQuests != null && personalQuests.SkipsDeconOnReturn(sv))
                return;
            if (sv?.Needs != null)
                if (_needsSystem != null)
                    _needsSystem.Modify(sv, NeedKind.Fatigue, 5f);
                else
                    sv.Needs.Fatigue = Mathf.Clamp(sv.Needs.Fatigue + 5f, 0f, 100f);
        }

        /// <summary>
        /// Prompt #234 — Rad-Walker returns: suit contamination sheds instantly, no decon.
        /// </summary>
        public bool TryRadWalkerBypassDecon(Survivors.Survivor sv, PersonalQuestSystem personalQuests)
        {
            if (sv == null || personalQuests == null || !personalQuests.SkipsDeconOnReturn(sv))
                return false;
            _airlockContamination = 0f;
            _scavengerInAirlock = false;
            _innerDoorSealed = true;
            return true;
        }

        public void SealInnerDoor() { _innerDoorSealed = true; }

        public AirlockSave CaptureState() => new AirlockSave { AirlockExists = _airlockExists, InnerDoorSealed = _innerDoorSealed, AirlockContamination = _airlockContamination, ScavengerInAirlock = _scavengerInAirlock };
        public void RestoreState(AirlockSave s) { if (s == null) return; _airlockExists = s.AirlockExists; _innerDoorSealed = s.InnerDoorSealed; _airlockContamination = s.AirlockContamination; _scavengerInAirlock = s.ScavengerInAirlock; }
    }
    [Serializable] public class AirlockSave { public bool AirlockExists, InnerDoorSealed, ScavengerInAirlock; public float AirlockContamination; }
}
