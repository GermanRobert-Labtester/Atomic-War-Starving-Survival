using System;
using System.Collections.Generic;

namespace Ashfall.Core.Maritime
{
    public enum DiveRoomType { Deckhouse, Companionway, HoldApproach, DeepHold }

    [Serializable]
    public sealed class DiveRoomNode
    {
        public DiveRoomType roomType;
        public float searchProgress; // 0.0 to 100.0
        public bool isLooted;
        public int hazardLevel; // 1 to 5
    }

    [Serializable]
    public sealed class StealthDiveSaveState
    {
        public bool isActive;
        public string diverDwellerId;
        public string compressorOperatorDwellerId;
        public float airSupplySeconds;
        public float maxAirSupplySeconds;
        public int currentRoomIndex;
        public int noiseLevel; // 0 to 100
        public bool isCompromised;
        public List<DiveRoomNode> rooms = new List<DiveRoomNode>();
    }

    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — 4-Room Stealth Dive Instance.
    /// Manages underwater salvage, surface air compressor delivery, and noise/stealth triage.
    /// </summary>
    public sealed class StealthDiveInstance
    {
        public const float BaseAirPerCrank = 30f; // Seconds of air gained per manual operator crank

        public bool IsActive { get; private set; }
        public string DiverDwellerId { get; private set; } = string.Empty;
        public string CompressorOperatorDwellerId { get; private set; } = string.Empty;
        public float AirSupplySeconds { get; private set; }
        public float MaxAirSupplySeconds { get; private set; } = 120f;
        public int CurrentRoomIndex { get; private set; }
        public int NoiseLevel { get; private set; }
        public bool IsCompromised { get; private set; }
        private bool _airWarningFired;

        private readonly List<DiveRoomNode> _rooms = new List<DiveRoomNode>();
        public IReadOnlyList<DiveRoomNode> Rooms => _rooms;

        public event Action<float> OnAirWarning;
        public event Action<int> OnRoomEntered;
        public event Action<bool> OnDiveEnded;

        public void StartDive(string diverId, string operatorId, float initialAir = 120f)
        {
            DiverDwellerId = diverId ?? string.Empty;
            CompressorOperatorDwellerId = operatorId ?? string.Empty;
            MaxAirSupplySeconds = Math.Max(30f, initialAir);
            AirSupplySeconds = MaxAirSupplySeconds;
            CurrentRoomIndex = 0;
            NoiseLevel = 0;
            IsCompromised = false;
            _airWarningFired = false;
            IsActive = true;

            _rooms.Clear();
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.Deckhouse, hazardLevel = 1 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.Companionway, hazardLevel = 2 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.HoldApproach, hazardLevel = 3 });
            _rooms.Add(new DiveRoomNode { roomType = DiveRoomType.DeepHold, hazardLevel = 4 });

            OnRoomEntered?.Invoke(0);
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsActive) return;

            AirSupplySeconds = Math.Max(0f, AirSupplySeconds - deltaSeconds);
            if (AirSupplySeconds <= 30f && !_airWarningFired)
            {
                _airWarningFired = true;
                OnAirWarning?.Invoke(AirSupplySeconds);
            }

            if (AirSupplySeconds <= 0f)
            {
                EndDive(success: false);
            }
        }

        public void CrankCompressor()
        {
            if (!IsActive) return;
            AirSupplySeconds = Math.Min(MaxAirSupplySeconds, AirSupplySeconds + BaseAirPerCrank);
        }

        public bool AdvanceToNextRoom(int addedNoise)
        {
            if (!IsActive) return false;
            if (CurrentRoomIndex >= _rooms.Count - 1) return false;

            CurrentRoomIndex++;
            NoiseLevel = MathfCompat.Clamp(NoiseLevel + addedNoise, 0, 100);
            if (NoiseLevel >= 80)
            {
                IsCompromised = true;
            }

            OnRoomEntered?.Invoke(CurrentRoomIndex);
            return true;
        }

        public void EndDive(bool success)
        {
            IsActive = false;
            OnDiveEnded?.Invoke(success);
        }

        public StealthDiveSaveState CaptureState()
        {
            var save = new StealthDiveSaveState
            {
                isActive = IsActive,
                diverDwellerId = DiverDwellerId,
                compressorOperatorDwellerId = CompressorOperatorDwellerId,
                airSupplySeconds = AirSupplySeconds,
                maxAirSupplySeconds = MaxAirSupplySeconds,
                currentRoomIndex = CurrentRoomIndex,
                noiseLevel = NoiseLevel,
                isCompromised = IsCompromised
            };
            foreach (var r in _rooms)
            {
                save.rooms.Add(new DiveRoomNode
                {
                    roomType = r.roomType,
                    searchProgress = r.searchProgress,
                    isLooted = r.isLooted,
                    hazardLevel = r.hazardLevel
                });
            }
            return save;
        }

        public void RestoreState(StealthDiveSaveState state)
        {
            _rooms.Clear();
            if (state == null) return;

            IsActive = state.isActive;
            DiverDwellerId = state.diverDwellerId ?? string.Empty;
            CompressorOperatorDwellerId = state.compressorOperatorDwellerId ?? string.Empty;
            AirSupplySeconds = state.airSupplySeconds;
            MaxAirSupplySeconds = state.maxAirSupplySeconds > 0 ? state.maxAirSupplySeconds : 120f;
            CurrentRoomIndex = MathfCompat.Clamp(state.currentRoomIndex, 0, MathfCompat.Max(0, state.rooms.Count - 1));
            NoiseLevel = MathfCompat.Clamp(state.noiseLevel, 0, 100);
            IsCompromised = state.isCompromised;
            _airWarningFired = state.airSupplySeconds <= 30f;

            if (state.rooms != null)
            {
                foreach (var r in state.rooms)
                {
                    _rooms.Add(new DiveRoomNode
                    {
                        roomType = r.roomType,
                        searchProgress = r.searchProgress,
                        isLooted = r.isLooted,
                        hazardLevel = r.hazardLevel
                    });
                }
            }
        }
    }
}
