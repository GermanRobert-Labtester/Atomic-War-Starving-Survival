using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ZoonoticFluState
    {
        public string afflictionId = "affliction_zoonotic_flu";
        public float contagionRate = 0.8f;
        public bool spreadsViaVents = true;
        public bool isQuarantined = false;
        public bool isInfected = false;
        public string sourceAnimal = string.Empty;
        public string infectedSurvivorId = string.Empty;
    }

    public class Affliction_ZoonoticFlu
    {
        /// <summary>
        /// MISC-005: seeded stream so this system's rolls replay identically. The
        /// call sites below previously used wall-clock UnityEngine.Random, which made
        /// the same save produce different outcomes on each load.
        /// </summary>
        private static System.Random _fallbackRng;
    private static System.Random FallbackRng =>
        _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("affliction_zoonoticflu");

        public event Action<string, string> OnInfectionStarted;     // survivorId, sourceAnimal
        public event Action<string, string> OnSpreadViaVents;       // infectedId, newlyInfectedId

        private ZoonoticFluState _state;

        public Affliction_ZoonoticFlu(ZoonoticFluState state = null)
        {
            _state = state ?? new ZoonoticFluState();
        }

        public string AfflictionId => _state.afflictionId;

        public void Contract(string survivorId, string sourceAnimal)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Affliction_ZoonoticFlu] Contract called with null/empty survivorId.");
                return;
            }

            _state.infectedSurvivorId = survivorId;
            _state.sourceAnimal = sourceAnimal ?? string.Empty;
            _state.isInfected = true;
            _state.isQuarantined = false;

            OnInfectionStarted?.Invoke(survivorId, _state.sourceAnimal);
        }

        public void TickHour(string infectedId, List<string> sharedRoomSurvivors, bool ventsConnected)
        {
            if (string.IsNullOrEmpty(infectedId))
            {
                Debug.LogWarning("[Affliction_ZoonoticFlu] TickHour called with null/empty infectedId.");
                return;
            }

            // Don't spread if quarantined
            if (_state.isQuarantined)
            {
                return;
            }

            // Spread via ventilation system if connected
            if (_state.spreadsViaVents && ventsConnected && sharedRoomSurvivors != null)
            {
                foreach (string survivorId in sharedRoomSurvivors)
                {
                    if (!string.IsNullOrEmpty(survivorId) && survivorId != infectedId)
                    {
                        // High contagion rate through vents
                        if (FallbackRng.NextDouble() < _state.contagionRate)
                        {
                            OnSpreadViaVents?.Invoke(infectedId, survivorId);
                        }
                    }
                }
            }
        }

        public void Quarantine(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Affliction_ZoonoticFlu] Quarantine called with null/empty survivorId.");
                return;
            }

            if (_state.infectedSurvivorId == survivorId)
            {
                _state.isQuarantined = true;
            }
        }

        public bool IsInfected() => _state.isInfected;
        public bool IsQuarantined() => _state.isQuarantined;
        public string GetSourceAnimal() => _state.sourceAnimal;
        public string GetInfectedSurvivorId() => _state.infectedSurvivorId;

        public ZoonoticFluState CaptureState()
        {
            return new ZoonoticFluState
            {
                afflictionId = _state.afflictionId,
                contagionRate = _state.contagionRate,
                spreadsViaVents = _state.spreadsViaVents,
                isQuarantined = _state.isQuarantined,
                isInfected = _state.isInfected,
                sourceAnimal = _state.sourceAnimal,
                infectedSurvivorId = _state.infectedSurvivorId
            };
        }

        public void RestoreState(ZoonoticFluState state)
        {
            _state = state ?? new ZoonoticFluState();
        }
    }
}
