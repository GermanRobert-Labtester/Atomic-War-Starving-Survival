using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PhantomBlipState
    {
        public string eventId = "ui_event_phantom_blip";
        public bool isActive;
        public int phantomHordeSize;
        public string phantomDirection;
        public float durationMinutes = 5f;
        public float radiationAnxietyThreshold = 80f;
    }

    public struct PhantomBlipDisplayData
    {
        public int hordeSize;
        public string direction;
        public string threatLabel;
    }

    public class PhantomBlipSystem
    {
        private const int MinHordeSize = 40;
        private const int MaxHordeSize = 120;

        private static readonly string[] Directions = { "north", "south", "east", "west", "northeast", "northwest" };
        private static readonly string[] ThreatLabels =
        {
            "MASSIVE HOSTILE FORMATION",
            "UNIDENTIFIED HORDE — APPROACHING FAST",
            "RADIATION SIGNATURE + MOVEMENT — LARGE GROUP",
            "SIGNAL LOST — THEY'RE CLOSER THAN WE THOUGHT"
        };

        private PhantomBlipState _state = new PhantomBlipState();

        public PhantomBlipState State => _state;

        public event Action<int, string> OnPhantomBlipSpawned;  // hordeSize, direction
        public event Action OnPhantomBlipExpired;

        public bool CheckActivation(float currentRadiationAnxiety, System.Random rng)
        {
            if (_state.isActive)
                return false;

            if (currentRadiationAnxiety < _state.radiationAnxietyThreshold)
                return false;

            _state.isActive = true;
            _state.phantomHordeSize = rng.Next(MinHordeSize, MaxHordeSize + 1);
            _state.phantomDirection = Directions[rng.Next(Directions.Length)];

            OnPhantomBlipSpawned?.Invoke(_state.phantomHordeSize, _state.phantomDirection);
            return true;
        }

        public bool IsPhantom()
        {
            return true; // Always phantom — never real
        }

        public PhantomBlipDisplayData GetDisplayData(System.Random rng)
        {
            return new PhantomBlipDisplayData
            {
                hordeSize = _state.phantomHordeSize,
                direction = _state.phantomDirection,
                threatLabel = ThreatLabels[rng.Next(ThreatLabels.Length)]
            };
        }

        public void Expire()
        {
            if (!_state.isActive)
                return;

            _state.isActive = false;
            _state.phantomHordeSize = 0;
            _state.phantomDirection = string.Empty;
            OnPhantomBlipExpired?.Invoke();
        }

        public void TickMinute()
        {
            if (!_state.isActive)
                return;

            _state.durationMinutes -= 1f;
            if (_state.durationMinutes <= 0f)
            {
                Expire();
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PhantomBlipState CaptureState() => _state;

        public void RestoreState(PhantomBlipState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
